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
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.FamilyService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemCommand
{
    [InjectableService]
    public class CraftingItemCommand : ICraftingItemCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICraftingService _craftingService;
        private readonly IIngredientService _ingredientService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly ILocationService _locationService;
        private readonly IBuildingService _buildingService;
        private readonly IPropertyService _propertyService;
        private readonly IUserService _userService;
        private readonly IFamilyService _familyService;
        private readonly ICalculationService _calc;
        private readonly IImageService _imageService;

        public CraftingItemCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICraftingService craftingService, IIngredientService ingredientService, IInventoryService inventoryService,
            ILocalizationService local, ILocationService locationService, IBuildingService buildingService,
            IPropertyService propertyService, IUserService userService, IFamilyService familyService,
            ICalculationService calc, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _craftingService = craftingService;
            _ingredientService = ingredientService;
            _inventoryService = inventoryService;
            _local = local;
            _locationService = locationService;
            _buildingService = buildingService;
            _propertyService = propertyService;
            _userService = userService;
            _familyService = familyService;
            _calc = calc;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context, long amount, long craftingId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем изготавливаемый предмет
            var crafting = await _craftingService.GetCrafting(craftingId);
            // получаем текущую локацию пользователя
            var userLocation = await _locationService.GetUserLocation((long) context.User.Id);

            // проверяем что пользователь находится в нужной для изготовления локации
            await CheckCraftingLocation((long) context.User.Id, userLocation, crafting.Location);
            // проверяем что у пользователя есть все необходимые для изготовления ингредиенты
            await _ingredientService.CheckCraftingIngredients((long) context.User.Id, crafting.Id);

            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
            // проверяем нужно ли ему оплачивать стоимость изготовления
            var freeCrafting = await CheckFreeCrafting((long) context.User.Id, userLocation);
            // считаем стоимость изготовления
            var craftingPrice = freeCrafting
                ? 0
                : await _calc.CraftingPrice(
                    await _ingredientService.GetCraftingCostPrice(crafting.Id), amount);

            // проверяем есть ли у пользователя деньги на оплату стоимости изготовления
            if (userCurrency.Amount < craftingPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
            }
            else
            {
                // получаем текущее время
                var timeNow = DateTimeOffset.Now;
                // определяем длительность изготовления
                var craftingTime = await CalculateCraftingTime(
                    (long) context.User.Id, userLocation, crafting.Time, amount);

                // отнимаем у пользователя ингредиенты для изготовления
                await _ingredientService.RemoveCraftingIngredients((long) context.User.Id, crafting.Id, amount);
                // обновляем пользователю текущую локацию
                await _locationService.UpdateUserLocation((long) context.User.Id, Location.MakingCrafting);
                // добавляем информацию о перемещении
                await _locationService.AddUserMovement(
                    (long) context.User.Id, Location.MakingCrafting, userLocation,
                    timeNow.AddSeconds(craftingTime));
                // отнимаем энергию у пользователя
                await _userService.RemoveEnergyFromUser((long) context.User.Id,
                    // получаем количество энергии
                    await _propertyService.GetPropertyValue(Property.EnergyCostCraft) * amount);
                // забираем у пользователя деньги на оплату стоимости изготовления
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), craftingPrice);

                // запускаем джобу завершения изготовления предмета
                BackgroundJob.Schedule<IMakingJob>(x =>
                        x.CompleteCrafting((long) context.User.Id, crafting.Id, amount, userLocation),
                    TimeSpan.FromSeconds(craftingTime));

                var buildingWorkshop = await _buildingService.GetBuilding(Building.Workshop);
                var embed = new EmbedBuilder()
                    // баннер изготовления
                    .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
                    .WithAuthor(Location.MakingCrafting.Localize())
                    // подверждаем что изготовление начато
                    .WithDescription(
                        IzumiReplyMessage.CraftingResourceDesc.Parse(
                            emotes.GetEmoteOrBlank(crafting.Name), _local.Localize(crafting.Name, 5)) +
                        (craftingPrice == 0
                            ? IzumiReplyMessage.CraftingResourceInFamilyHouse.Parse(
                                emotes.GetEmoteOrBlank(buildingWorkshop.Name),
                                buildingWorkshop.Name)
                            : "") +
                        $"\n{emotes.GetEmoteOrBlank("Blank")}")
                    // потраченные ингредиенты
                    .AddField(IzumiReplyMessage.IngredientsSpent.Parse(),
                        await _ingredientService.DisplayCraftingIngredients(crafting.Id, amount) +
                        (craftingPrice == 0
                            ? ""
                            : $", {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {craftingPrice} {_local.Localize(Currency.Ien.ToString(), craftingPrice)}"
                        ), true)
                    // длительность
                    .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                        craftingTime.Seconds().Humanize(2, new CultureInfo("ru-RU")), true)
                    // ожидаемые предметы
                    .AddField(IzumiReplyMessage.CraftingResourceExpectedFieldName.Parse(),
                        $"{emotes.GetEmoteOrBlank(crafting.Name)} {amount} {_local.Localize(crafting.Name, amount)}");

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }

        public async Task Execute(SocketCommandContext context, long amount, string itemNamePattern)
        {
            // получаем локализацию предмета
            var itemLocalization = await _local.GetLocalizationByLocalizedWord(
                LocalizationCategory.Crafting, itemNamePattern);
            // используем основной метод уже зная id предмета
            await Execute(context, amount, itemLocalization.ItemId);
        }

        private async Task CheckCraftingLocation(long userId, Location userLocation, Location craftingLocation)
        {
            // если пользователь находится в локации где изготавливается предмет - все ок
            if (userLocation == craftingLocation) return;

            // если пользователь находится в деревне, нужно проверить есть ли у него семья и есть ли у семьи мастерская
            if (userLocation == Location.Village)
            {
                // проверяем состоит ли пользователь в семье
                var hasFamily = await _familyService.CheckUserHasFamily(userId);

                if (hasFamily)
                {
                    // получаем семью пользователя
                    var userFamily = await _familyService.GetUserFamily(userId);
                    // проверяем есть ли у его семьи мастерская
                    var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                        userFamily.FamilyId, Building.Workshop);

                    // если есть - все ок
                    if (familyHasBuilding) return;

                    // если нет - он не может изготовить предмет в деревне
                    await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                        craftingLocation.Localize(true))));
                }
                // если пользователь не состоит в семье - он не может изготовить предмет в деревне
                else
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                        craftingLocation.Localize(true))));
                }
            }
            // если пользователь находится в другой локации - он не может изготовить предмет
            else
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ResourceCraftWrongLocation.Parse(
                    craftingLocation.Localize(true))));
            }
        }

        private async Task<bool> CheckFreeCrafting(long userId, Location userLocation)
        {
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily(userId);

            // если пользователь не состоит в семье - он должен платить стоимость изготовления
            if (!hasFamily) return false;

            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily(userId);
            // проверяем есть ли у его семьи мастерская
            var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                userFamily.FamilyId, Building.Workshop);

            // если у семьи пользователя есть мастерская и он находится в деревне - он может изготовить предмет бесплатно
            return familyHasBuilding && userLocation == Location.Village;
        }

        private async Task<long> CalculateCraftingTime(long userId, Location userLocation, long craftingTime,
            long amount)
        {
            // получаем пользователя
            var user = await _userService.GetUser(userId);
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _familyService.CheckUserHasFamily(user.Id);

            // если не состоит - возвращаем длительность изготовления без учета постройки
            if (!hasFamily) return _calc.ActionTime(craftingTime * amount, user.Energy);

            // получаем семью пользователя
            var userFamily = await _familyService.GetUserFamily(user.Id);
            // проверяем наличие у семьи мастерской
            var familyHasBuilding = await _buildingService.CheckBuildingInFamily(
                userFamily.FamilyId, Building.Workshop);

            // если у семьи нет мастерской или пользователь не находится в деревне
            if (!familyHasBuilding || userLocation != Location.Village)
                // возвращаем длительность изготовления без учета постройки
                return _calc.ActionTime(craftingTime * amount, user.Energy);

            // считаем стандартное время изготовления с учетом постройки
            var defaultTime = craftingTime * amount - craftingTime * amount /
                await _propertyService.GetPropertyValue(Property.ActionTimeReduceWorkshop) * 100;
            // возвращаем длительность изготовления
            return _calc.ActionTime(defaultTime, user.Energy);
        }
    }
}
