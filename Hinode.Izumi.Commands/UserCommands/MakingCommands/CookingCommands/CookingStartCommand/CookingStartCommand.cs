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
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BuildingService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Commands;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingStartCommand
{
    [InjectableService]
    public class CookingStartCommand : ICookingStartCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CookingStartCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long amount, long foodId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем приготавливаемое блюдо
            var food = await _mediator.Send(new GetFoodQuery(foodId));
            // проверяем есть ли у пользователя рецепт
            var hasRecipe = await _mediator.Send(new CheckUserHasRecipeQuery((long) context.User.Id, food.Id));

            // если у пользователя нет рецепта - выводим ошибку
            if (!hasRecipe)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.CookingRecipeNull.Parse(
                    emotes.GetEmoteOrBlank("Recipe"))));
            }
            else
            {
                // получаем текущую локацию пользователя
                var userLocation = await _mediator.Send(new GetUserLocationQuery((long) context.User.Id));

                // проверяем что пользователь находится в нужной для приготовления локации
                await CheckCookingLocation((long) context.User.Id, userLocation, Location.Garden);
                // проверяем что у пользователя есть все необходимые для приготовления ингредиенты
                await _mediator.Send(new CheckFoodIngredientsCommand((long) context.User.Id, food.Id));

                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));
                // проверяем нужно ли ему оплачивать стоимость приготовления
                var freeCooking = await CheckFreeCooking((long) context.User.Id, userLocation);
                // считаем стоимость приготовления
                var cookingPrice = freeCooking
                    ? 0
                    : await _mediator.Send(new GetCraftingPriceQuery(
                        await _mediator.Send(new GetFoodCostPriceQuery(food.Id)), amount));

                // проверяем что у пользователя хватит денег на оплату приготовления
                if (userCurrency.Amount < cookingPrice)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.CookingNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // получаем текущее время
                    var timeNow = DateTimeOffset.Now;
                    // определяем длительность приготовления
                    var cookingTime = await CalculateCookingTime(
                        (long) context.User.Id, userLocation, food.Time, amount);

                    // отнимаем у пользователя ингредиенты для приготовления
                    await _mediator.Send(new RemoveFoodIngredientsCommand((long) context.User.Id, foodId, amount));
                    // обновляем пользователю текущую локацию
                    await _mediator.Send(new UpdateUserLocationCommand((long) context.User.Id, Location.MakingFood));
                    // добавляем информацию о перемещении
                    await _mediator.Send(new CreateUserMovementCommand(
                        (long) context.User.Id, Location.MakingFood, userLocation, timeNow.AddSeconds(cookingTime)));
                    // отнимаем энергию у пользователя
                    await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                        // получаем количество энергии
                        await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostCraft)) * amount));
                    // отнимаем у пользователя валюту для оплаты стоимости приготовления
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), cookingPrice));

                    // запускаем джобу завершения приготовления
                    var jobId = BackgroundJob.Schedule<IMakingJob>(x =>
                            x.CompleteFood((long) context.User.Id, food.Id, amount, userLocation),
                        TimeSpan.FromSeconds(cookingTime));
                    await _mediator.Send(new CreateUserHangfireJobCommand(
                        (long) context.User.Id, HangfireAction.Making, jobId));

                    var buildingKitchen = await _mediator.Send(new GetBuildingByTypeQuery(Building.Kitchen));
                    var embed = new EmbedBuilder()
                        // баннер приготовления
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Cooking)))
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
                            await _mediator.Send(new DisplayFoodIngredientsQuery(food.Id, amount)) +
                            (cookingPrice == 0
                                ? ""
                                : $", {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {cookingPrice} {_local.Localize(Currency.Ien.ToString(), cookingPrice)}"
                            ))
                        // ожидаемая еда
                        .AddField(IzumiReplyMessage.CraftingFoodExpectedFieldName.Parse(),
                            $"{emotes.GetEmoteOrBlank(food.Name)} {amount} {_local.Localize(food.Name, amount)}", true)
                        // длительность
                        .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                            cookingTime.Seconds().Humanize(2, new CultureInfo("ru-RU")), true);

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
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
                var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(userId));

                if (hasFamily)
                {
                    // получаем семью пользователя
                    var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
                    // проверяем есть ли у его семьи кухня
                    var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                        userFamily.FamilyId, Building.Kitchen));

                    // если есть - все ок
                    if (familyHasBuilding) return;

                    // если нет - он не может приготовить блюло в деревне
                    await Task.FromException(new Exception(IzumiReplyMessage.CookingWrongLocation.Parse(
                        cookingLocation.Localize(true))));
                }
                // если пользователь не состоит в семье - он не может приготовить блюдо в деревне
                else
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.CookingWrongLocation.Parse(
                        cookingLocation.Localize(true))));
                }
            }
            // если пользователь находится в другой локации - он не может приготовить блюдо
            else
            {
                await Task.FromException(new Exception(IzumiReplyMessage.CookingWrongLocation.Parse(
                    cookingLocation.Localize(true))));
            }
        }

        private async Task<bool> CheckFreeCooking(long userId, Location userLocation)
        {
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(userId));

            // если пользователь не состоит в семье - он должен платить стоимость изготовления
            if (!hasFamily) return false;

            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
            // проверяем есть ли у его семьи кухня
            var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                userFamily.FamilyId, Building.Kitchen));

            // если у семьи пользователя есть кухня и он находится в деревне - он может приготовить блюдо бесплатно
            return familyHasBuilding && userLocation == Location.Village;
        }

        private async Task<long> CalculateCookingTime(long userId, Location userLocation, long foodTime, long amount)
        {
            // получаем пользователя
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(user.Id));

            // если не состоит - возвращаем длительность приготовления без учета постройки
            if (!hasFamily) return await _mediator.Send(new GetActionTimeQuery(foodTime * amount, user.Energy));

            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery(user.Id));
            // проверяем наличие у семьи кухни
            var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                userFamily.FamilyId, Building.Kitchen));

            // если у семьи нет кухни или пользователь не находится в деревне
            if (!familyHasBuilding || userLocation != Location.Village)
                // возвращаем длительность приготовления без учета постройки
                return await _mediator.Send(new GetActionTimeQuery(foodTime * amount, user.Energy));

            // считаем стандартное время приготовления с учетом постройки
            var defaultTime = foodTime * amount - foodTime * amount /
                await _mediator.Send(new GetPropertyValueQuery(Property.ActionTimeReduceKitchen)) * 100;
            // возвращаем длительность приготовления
            return await _mediator.Send(new GetActionTimeQuery(defaultTime, user.Energy));
        }
    }
}
