using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.CooldownService.Commands;
using Hinode.Izumi.Services.GameServices.CooldownService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using Humanizer;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.CasinoCommands
{
    [CommandCategory(CommandCategory.Casino)]
    [IzumiRequireRegistry, IzumiRequireCasinoOpen]
    [IzumiRequireLocation(Location.CapitalCasino), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
    public class BetCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public BetCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("ставка"), Alias("bet")]
        [Summary("Сделать ставку в казино")]
        [CommandUsage("!ставка 200", "!ставка 1000")]
        public async Task BetTask(
            [Summary("Сумма ставки")] long amount = 0)
        {
            // получаем текущее время
            var timeNow = DateTimeOffset.Now;
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем кулдаун на ставку пользователя
            var userCooldown = await _mediator.Send(
                new GetUserCooldownQuery((long) Context.User.Id, Cooldown.GamblingBet));

            // проверяем может ли пользователь сделать сейчас ставку
            if (userCooldown.Expiration > timeNow)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetCooldown.Parse(
                    userCooldown.Expiration
                        .Subtract(timeNow).TotalMinutes.Minutes()
                        .Humanize(1, new CultureInfo("ru-RU")))));
            }
            else
            {
                // получаем минимальную ставку
                var minBet = await _mediator.Send(new GetPropertyValueQuery(Property.BinBet));
                // получаем максимальную ставку
                var maxBet = await _mediator.Send(new GetPropertyValueQuery(Property.MaxBet));

                // проверяем что пользователь указал сумму
                if (amount == 0)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetNoAmount.Parse()));
                }
                // проверяем что указанная пользователем сумма не меньше минимальной ставки
                else if (amount < minBet)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetMinCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), minBet)));
                }
                // проверяем что указанная пользователем сумма не больше максимальной ставки
                else if (amount > maxBet)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetMaxCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()), maxBet)));
                }
                else
                {
                    // получаем валюту пользователя
                    var userCurrency = await _mediator.Send(
                        new GetUserCurrencyQuery((long) Context.User.Id, Currency.Ien));

                    // проверяем есть ли у пользователя деньги на оплату ставки
                    if (userCurrency.Amount < amount)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetNoCurrency.Parse()));
                    }
                    else
                    {
                        // бросаем первый кубик
                        double firstDrop = new Random().Next(1, 101);
                        // бросаем второй кубик
                        double secondDrop = new Random().Next(1, 101);
                        // итоговый результат это среднее значение между первым и вторым кубиком
                        var cubeDrop = (long) Math.Floor((firstDrop + secondDrop) / 2);
                        // выводим итоговый результат
                        var cubeDropString = IzumiReplyMessage.GamblingBetCubeDrop.Parse(cubeDrop);

                        switch (cubeDrop)
                        {
                            // победа с х2 наградой
                            case >= 55 and < 90:

                                cubeDropString += IzumiReplyMessage.GamblingBetWon.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 2,
                                    _local.Localize(Currency.Ien.ToString(), amount * 2));

                                // добавляем пользователю валюту за победу
                                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount));

                                break;
                            // победа с х4 наградой
                            case >= 90 and < 100:

                                cubeDropString += IzumiReplyMessage.GamblingBetWon.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 4,
                                    _local.Localize(Currency.Ien.ToString(), amount * 4));

                                // добавляем пользователю валюту за победу
                                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount * 3));

                                break;
                            // джек-пот
                            case 100:

                                cubeDropString += IzumiReplyMessage.GamblingBetWon.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 10,
                                    _local.Localize(Currency.Ien.ToString(), amount * 10));

                                // добавляем пользователю валюту за победу
                                await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount * 9));
                                // добавляем пользователю статистику джек-потов
                                await _mediator.Send(new AddStatisticToUserCommand(
                                    (long) Context.User.Id, Statistic.CasinoJackPot));
                                // проверяем выполнил ли пользователь достижение
                                await _mediator.Send(new CheckAchievementInUserCommand(
                                    (long) Context.User.Id, Achievement.FirstJackPot));

                                break;
                            // поражение
                            default:

                                cubeDropString += IzumiReplyMessage.GamblingBetLose.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount,
                                    _local.Localize(Currency.Ien.ToString(), amount));

                                // отнимаем у пользователя валюту за поражение
                                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount));

                                break;
                        }

                        // добавляем пользователю кулдаун на ставку
                        await _mediator.Send(new AddCooldownToUserCommand(
                            (long) Context.User.Id, Cooldown.GamblingBet, timeNow.AddMinutes(
                                await _mediator.Send(new GetPropertyValueQuery(Property.CooldownCasinoBet)))));
                        // добавляем пользователю статистку сделанных бросков
                        await _mediator.Send(new AddStatisticToUserCommand(
                            (long) Context.User.Id, Statistic.CasinoBet));
                        // проверяем выполнил ли пользователь достижение
                        await _mediator.Send(new CheckAchievementsInUserCommand((long) Context.User.Id, new[]
                        {
                            Achievement.Casino33Bet,
                            Achievement.Casino777Bet
                        }));

                        var embed = new EmbedBuilder()
                            .WithDescription(cubeDropString);

                        await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
