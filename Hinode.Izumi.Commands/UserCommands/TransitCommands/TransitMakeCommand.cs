using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.TransitCommands
{
    [CommandCategory(CommandCategory.Transit)]
    [IzumiRequireRegistry]
    public class TransitMakeCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public TransitMakeCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("отправиться"), Alias("transit")]
        [Summary("Отправиться в указанную локацию")]
        [CommandUsage("!отправиться 1", "!отправиться 5")]
        public async Task TransitMakeTask(
            [Summary("Номер локации")] Location destination = 0)
        {
            // получаем пользователя из базы
            var user = await _mediator.Send(new GetUserByIdQuery((long) Context.User.Id));
            // получаем данные о перемещении пользователя
            var userMovement = await _mediator.Send(new GetUserMovementQuery((long) Context.User.Id));

            // если пользователь находится в процессе перемещения - он не может отправиться
            if (userMovement is not null)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeAlready.Parse(
                    userMovement.Destination.Localize())));
            }
            // проверяем что пользователь указал точку назначения
            else if (destination == Location.InTransit)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeNull.Parse()));
            }
            // точка назначения не может быть текущей локацией пользователя
            else if (destination == user.Location)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeCurrent.Parse()));
            }
            else
            {
                // получаем иконки из базы
                var emotes = await _mediator.Send(new GetEmotesQuery());
                // получаем информацию о перемещении
                var transit = await _mediator.Send(new GetTransitQuery(user.Location, destination));
                // получаем мастерство торговли пользователя
                var userMastery = await _mediator.Send(new GetUserMasteryQuery(
                    (long) Context.User.Id, Mastery.Trading));
                // округляем мастерство пользователя
                var userRoundedMastery = (long) Math.Round(userMastery.Amount);
                // определяем стоимость перемещения
                var transitCost =
                    transit.Price > 0
                        ? userRoundedMastery > 50
                            ? await _mediator.Send(new GetTransitCostWithDiscountQuery(
                                userRoundedMastery, transit.Price))
                            : transit.Price
                        : 0;
                // заполняем строку стоимости перемещения в зависимости от его стоимости
                var transitCostString =
                    transit.Price > 0
                        ? userRoundedMastery > 50
                            ? $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} ~~{transit.Price}~~ {transitCost} {_local.Localize(Currency.Ien.ToString(), transitCost)}"
                            : $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {transitCost} {_local.Localize(Currency.Ien.ToString(), transitCost)}"
                        : $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} бесплатно";
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) Context.User.Id, Currency.Ien));

                // проверяем хватает ли пользователю на оплату перемещения
                if (userCurrency.Amount < transitCost)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.TransitMakeNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), transitCost)));
                }
                else
                {
                    var transitTime = await _mediator.Send(new GetActionTimeQuery(transit.Time, user.Energy));
                    var currentEvent = (Event) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentEvent));
                    var transitTimeReduceEvent = await _mediator.Send(new GetPropertyValueQuery(
                        Property.EventReduceTransitTime));
                    var transitTimeReducePremium = await _mediator.Send(new GetPropertyValueQuery(
                        Property.TransitTimePercentReducePremium));

                    if (currentEvent != Event.None) transitTime -= transitTime * transitTimeReduceEvent / 100;
                    if (user.Premium) transitTime -= transitTime * transitTimeReducePremium / 100;

                    // начинаем транзит
                    await _mediator.Send(new BeginUserTransitCommand(
                        (long) Context.User.Id, user.Location, destination, transitTime));

                    var embed = new EmbedBuilder()
                        // баннер перемещения
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.InTransit)))
                        // подверждаем что пемерещение успешно начато
                        .WithDescription(
                            IzumiReplyMessage.TransitMakeSuccess.Parse(
                                emotes.GetEmoteOrBlank(Currency.Ien.ToString()), transitCost,
                                destination.Localize(), transitTime) +
                            $"\n{emotes.GetEmoteOrBlank("Blank")}")
                        // длительность
                        .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                            transitTime.Minutes().Humanize(2, new CultureInfo("ru-RU")), true)
                        // стоимость перемещения
                        .AddField(IzumiReplyMessage.TransitCostFieldName.Parse(),
                            transitCostString, true);

                    // если перемещение было не в подлокацию
                    if (!destination.SubLocation())
                    {
                        // забираем у пользователя деньги на оплату перемещения
                        await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                            (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                            transitCost));
                        // забираем у пользователя энергию за перемещение
                        await _mediator.Send(new RemoveEnergyFromUserCommand((long) Context.User.Id,
                            await _mediator.Send(new GetPropertyValueQuery(Property.EnergyCostTransit))));
                    }

                    await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
