using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hangfire;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.BackgroundJobs.MakingJob;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.MasteryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand
{
    [InjectableService]
    public class CookingStartCommand : ICookingStartCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IMasteryService _masteryService;
        private readonly IImageService _imageService;
        private readonly IFoodService _foodService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocationService _locationService;
        private readonly IIngredientService _ingredientService;
        private readonly IBuildingService _buildingService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IFamilyService _familyService;
        private readonly ICalculationService _calc;

        public CookingStartCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IMasteryService masteryService, IImageService imageService,
            IFoodService foodService, IInventoryService inventoryService, ILocationService locationService,
            IIngredientService ingredientService, IBuildingService buildingService, TimeZoneInfo timeZoneInfo,
            IUserService userService, IPropertyService propertyService, IFamilyService familyService,
            ICalculationService calc)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _masteryService = masteryService;
            _imageService = imageService;
            _foodService = foodService;
            _inventoryService = inventoryService;
            _locationService = locationService;
            _ingredientService = ingredientService;
            _buildingService = buildingService;
            _timeZoneInfo = timeZoneInfo;
            _userService = userService;
            _propertyService = propertyService;
            _familyService = familyService;
            _calc = calc;
        }

        public async Task Execute(SocketCommandContext context, long foodId, long amount)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем приготавливаемое блюдо
            var food = await _foodService.GetFood(foodId);
            // получаем текущую локацию пользователя
            var userLocation = await _locationService.GetUserLocation((long) context.User.Id);

            // проверяем что пользователь находится в нужной для приготовления локации
            await CheckCookingLocation((long) context.User.Id, userLocation, Location.Garden);
            // проверяем что у пользователя есть все необходимые для приготовления ингредиенты
            await _ingredientService.CheckFoodIngredients((long) context.User.Id, food.Id);

            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
            // проверяем нужно ли ему оплачивать стоимость приготовления
            var freeCooking = await CheckFreeCooking((long) context.User.Id, userLocation);
            // считаем стоимость приготовления
            var cookingPrice = freeCooking
                ? 0
                : await _calc.CraftingPrice(
                    await _ingredientService.GetFoodCostPrice(food.Id));

            // проверяем что у пользователя хватит денег на оплату приготовления
            if (userCurrency.Amount < cookingPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.CookingNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // получаем текущее время
                var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
                // определяем длительность приготовления
                var cookingTime = await CalculateCookingTime(
                    (long) context.User.Id, userLocation, food.Time, amount);

                // отнимаем у пользователя ингредиенты для приготовления
                await _ingredientService.RemoveFoodIngredients((long) context.User.Id, foodId);
                // обновляем пользователю текущую локацию
                await _locationService.UpdateUserLocation((long) context.User.Id, Location.MakingFood);
                // добавляем информацию о перемещении
                await _locationService.AddUserMovement(
                    (long) context.User.Id, Location.MakingFood, userLocation, timeNow.AddSeconds(cookingTime));
                // отнимаем энергию у пользователя
                await _userService.RemoveEnergyFromUser((long) context.User.Id,
                    // получаем количество энергии
                    await _propertyService.GetPropertyValue(Property.EnergyCostCraft) * amount);
                // отнимаем у пользователя валюту для оплаты стоимости приготовления
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), cookingPrice);

                // запускаем джобу завершения приготовления
                BackgroundJob.Schedule<IMakingJob>(x =>
                        x.CompleteFood((long) context.User.Id, food.Id, amount, userLocation),
                    TimeSpan.FromSeconds(cookingTime));

                var buildingKitchen = await _buildingService.GetBuilding(Building.Kitchen);
                var embed = new EmbedBuilder()
                    // баннер приготовления
                    .WithImageUrl(await _imageService.GetImageUrl(Image.Cooking))
                    .WithAuthor(Location.MakingFood.Localize())
                    .WithDescription(
                        // подтверждаем что приготовление начато
                        IzumiReplyMessage.CraftingFoodDesc.Parse(
                            emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name)) +
                        (cookingPrice == 0
                            ? IzumiReplyMessage.CraftingFoodInFamilyHouse.Parse(
                                emotes.GetEmoteOrBlank(buildingKitchen.Type.ToString()),
                                buildingKitchen.Name)
                            : "") +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // потраченные ингредиенты
                    .AddField(IzumiReplyMessage.IngredientsSpent.Parse(),
                        await _ingredientService.DisplayFoodIngredients(food.Id, amount) +
                        (cookingPrice == 0
                            ? ""
                            : $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {cookingPrice} {_local.Localize(Currency.Ien.ToString(), cookingPrice)}"
                        ), true)
                    // длительность
                    .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                        cookingTime.Seconds().Humanize(2, new CultureInfo("ru-RU")), true)
                    // ожидаемая еда
                    .AddField(IzumiReplyMessage.CraftingFoodExpectedFieldName.Parse(),
                        $"{emotes.GetEmoteOrBlank(food.Name)} {amount} {_local.Localize(food.Name, amount)}");

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }

        private async Task CheckCookingLocation(long userId, Location userLocation, Location cookingLocation)
        {
            // если пользователь находится в локации где приготавливается блюдо - все ок
            if (userLocation == cookingLocation) return;

            // если пользователь находится в деревне, нужно проверить есть ли у него семья и есть ли у семьи кухня
            if (userLocation == Location.Village)
            {
                // проверяем состоит ли пользователь в семье
                var hasFamily = await _familyService.CheckUserHasFamily(userId);

                if (hasFamily)
                {
                    // получаем семью пользователя
                    var userFamily = await _familyService.GetUserFamily(userId);
                    // проверяем есть ли у его семьи кухня
                    var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                        userFamily.FamilyId, Building.Kitchen);

                    // если есть - все ок
                    if (familyHasBuilding) return;

                    // если нет - он не может приготовить блюло в деревне
                    await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                        cookingLocation.Localize(true))));
                }
                // если пользователь не состоит в семье - он не может приготовить блюдо в деревне
                else
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                        cookingLocation.Localize(true))));
                }
            }
            // если пользователь находится в другой локации - он не может приготовить блюдо
            else
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                    cookingLocation.Localize(true))));
            }
        }

        private async Task<bool> CheckFreeCooking(long userId, Location userLocation)
        {
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily(userId);

            // если пользователь не состоит в семье - он должен платить стоимость изготовления
            if (!hasFamily) return false;

            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily(userId);
            // проверяем есть ли у его семьи кухня
            var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                userFamily.FamilyId, Building.Kitchen);

            // если у семьи пользователя есть кухня и он находится в деревне - он может приготовить блюдо бесплатно
            return familyHasBuilding && userLocation == Location.Village;
        }

        private async Task<long> CalculateCookingTime(long userId, Location userLocation, long foodTime, long amount)
        {
            // получаем пользователя
            var user = await _userService.GetUser(userId);
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily(user.Id);

            // если не состоит - возвращаем длительность приготовления без учета постройки
            if (!hasFamily) return _calc.ActionTime(foodTime * amount, user.Energy);

            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily(user.Id);
            // проверяем наличие у семьи кухни
            var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                userFamily.FamilyId, Building.Kitchen);

            // если у семьи нет кухни или пользователь не находится в деревне
            if (!familyHasBuilding || userLocation != Location.Village)
                // возвращаем длительность приготовления без учета постройки
                return _calc.ActionTime(foodTime * amount, user.Energy);

            // считаем стандартное время приготовления с учетом постройки
            var defaultTime = foodTime * amount - foodTime * amount /
                await _propertyService.GetPropertyValue(Property.ActionTimeReduceKitchen) * 100;
            // возвращаем длительность приготовления
            return _calc.ActionTime(defaultTime, user.Energy);
        }
    }
}
