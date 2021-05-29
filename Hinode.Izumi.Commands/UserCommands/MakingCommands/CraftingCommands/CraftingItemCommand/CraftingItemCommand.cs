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
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
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
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemCommand
{
    [InjectableService]
    public class CraftingItemCommand : ICraftingItemCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingItemCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long amount, long craftingId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем изготавливаемый предмет
            var crafting = await _mediator.Send(new GetCraftingQuery(craftingId));
            // получаем текущую локацию пользователя
            var userLocation = await _mediator.Send(new GetUserLocationQuery((long) context.User.Id));

            // проверяем что пользователь находится в нужной для изготовления локации
            await CheckCraftingLocation((long) context.User.Id, userLocation, crafting.Location);
            // проверяем что у пользователя есть все необходимые для изготовления ингредиенты
            await _mediator.Send(new CheckCraftingIngredientsCommand((long) context.User.Id, crafting.Id));

            // получаем валюту пользователя
            var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));
            // проверяем нужно ли ему оплачивать стоимость изготовления
            var freeCrafting = await CheckFreeCrafting((long) context.User.Id, userLocation);
            // считаем стоимость изготовления
            var craftingPrice = freeCrafting
                ? 0
                : await _mediator.Send(new GetCraftingPriceQuery(
                    await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Id)), amount));

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
                await _mediator.Send(new RemoveCraftingIngredientsCommand((long) context.User.Id, crafting.Id, amount));
                // обновляем пользователю текущую локацию
                await _mediator.Send(new UpdateUserLocationCommand((long) context.User.Id, Location.MakingCrafting));
                // добавляем информацию о перемещении
                await _mediator.Send(new CreateUserMovementCommand(
                    (long) context.User.Id, Location.MakingCrafting, userLocation,
                    timeNow.AddSeconds(craftingTime)));
                // отнимаем энергию у пользователя
                await _mediator.Send(new RemoveEnergyFromUserCommand((long) context.User.Id,
                    // получаем количество энергии
                    await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostCraft)) * amount));
                // забираем у пользователя деньги на оплату стоимости изготовления
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), craftingPrice));

                // запускаем джобу завершения изготовления предмета
                BackgroundJob.Schedule<IMakingJob>(x =>
                        x.CompleteCrafting((long) context.User.Id, crafting.Id, amount, userLocation),
                    TimeSpan.FromSeconds(craftingTime));

                var buildingWorkshop = await _mediator.Send(new GetBuildingByTypeQuery(Building.Workshop));
                var embed = new EmbedBuilder()
                    // баннер изготовления
                    .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
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
                        await _mediator.Send(new DisplayCraftingIngredientQuery(crafting.Id, amount)) +
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

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
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
                var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(userId));

                if (hasFamily)
                {
                    // получаем семью пользователя
                    var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
                    // проверяем есть ли у его семьи мастерская
                    var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                        userFamily.FamilyId, Building.Workshop));

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
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(userId));

            // если пользователь не состоит в семье - он должен платить стоимость изготовления
            if (!hasFamily) return false;

            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery(userId));
            // проверяем есть ли у его семьи мастерская
            var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                userFamily.FamilyId, Building.Workshop));

            // если у семьи пользователя есть мастерская и он находится в деревне - он может изготовить предмет бесплатно
            return familyHasBuilding && userLocation == Location.Village;
        }

        private async Task<long> CalculateCraftingTime(long userId, Location userLocation, long craftingTime,
            long amount)
        {
            // получаем пользователя
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            // проверяем состоит ли пользователь в семье
            var hasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(user.Id));

            // если не состоит - возвращаем длительность изготовления без учета постройки
            if (!hasFamily) return await _mediator.Send(new GetActionTimeQuery(craftingTime * amount, user.Energy));

            // получаем семью пользователя
            var userFamily = await _mediator.Send(new GetUserFamilyQuery(user.Id));
            // проверяем наличие у семьи мастерской
            var familyHasBuilding = await _mediator.Send(new CheckBuildingInFamilyQuery(
                userFamily.FamilyId, Building.Workshop));

            // если у семьи нет мастерской или пользователь не находится в деревне
            if (!familyHasBuilding || userLocation != Location.Village)
                // возвращаем длительность изготовления без учета постройки
                return await _mediator.Send(new GetActionTimeQuery(craftingTime * amount, user.Energy));

            // считаем стандартное время изготовления с учетом постройки
            var defaultTime = craftingTime * amount - craftingTime * amount /
                await _mediator.Send(new GetPropertyValueQuery(Property.ActionTimeReduceWorkshop)) * 100;
            // возвращаем длительность изготовления
            return await _mediator.Send(new GetActionTimeQuery(defaultTime, user.Energy));
        }
    }
}
