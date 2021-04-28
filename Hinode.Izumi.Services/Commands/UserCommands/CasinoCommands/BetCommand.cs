using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.CooldownService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Humanizer;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    [IzumiRequireLocation(Location.CapitalCasino), IzumiRequireNoDebuff(BossDebuff.CapitalStop)]
    public class BetCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICooldownService _cooldownService;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly IInventoryService _inventoryService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IPropertyService _propertyService;

        public BetCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICooldownService cooldownService, TimeZoneInfo timeZoneInfo, IInventoryService inventoryService,
            IStatisticService statisticService, IAchievementService achievementService,
            IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _cooldownService = cooldownService;
            _timeZoneInfo = timeZoneInfo;
            _inventoryService = inventoryService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _propertyService = propertyService;
        }

        [Command("ставка"), Alias("bet")]
        public async Task BetTask(long amount = 0)
        {
            // получаем текущее время
            var timeNow = TimeZoneInfo.ConvertTime(DateTime.Now, _timeZoneInfo);
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем кулдаун на ставку пользователя
            var userCooldown = await _cooldownService.GetUserCooldown((long) Context.User.Id, Cooldown.GamblingBet);

            // проверяем может ли пользователь сделать сейчас ставку
            if (userCooldown.Expiration > timeNow)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.GamblingBetCooldown.Parse(
                    userCooldown.Expiration
                        .Subtract(timeNow).TotalMinutes.Minutes()
                        .Humanize(2, new CultureInfo("ru-RU")))));
            }
            else
            {
                // получаем минимальную ставку
                var minBet = await _propertyService.GetPropertyValue(Property.BinBet);
                // получаем максимальную ставку
                var maxBet = await _propertyService.GetPropertyValue(Property.MaxBet);

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
                    var userCurrency = await _inventoryService.GetUserCurrency((long) Context.User.Id, Currency.Ien);

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
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 2);

                                // добавляем пользователю валюту за победу
                                await _inventoryService.AddItemToUser(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount);

                                break;
                            // победа с х4 наградой
                            case >= 90 and < 100:

                                cubeDropString += IzumiReplyMessage.GamblingBetWon.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 4);

                                // добавляем пользователю валюту за победу
                                await _inventoryService.AddItemToUser(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount * 3);

                                break;
                            // джек-пот
                            case 100:

                                cubeDropString += IzumiReplyMessage.GamblingBetWon.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount * 10);

                                // добавляем пользователю валюту за победу
                                await _inventoryService.AddItemToUser(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount * 9);
                                // добавляем пользователю статистику джек-потов
                                await _statisticService.AddStatisticToUser(
                                    (long) Context.User.Id, Statistic.CasinoJackPot);
                                // проверяем выполнил ли пользователь достижение
                                await _achievementService.CheckAchievement(
                                    (long) Context.User.Id, Achievement.FirstJackPot);

                                break;
                            // поражение
                            default:

                                cubeDropString += IzumiReplyMessage.GamblingBetLose.Parse(
                                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), amount);

                                // отнимаем у пользователя валюту за поражение
                                await _inventoryService.RemoveItemFromUser(
                                    (long) Context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                                    amount);

                                break;
                        }

                        // добавляем пользователю кулдаун на ставку
                        await _cooldownService.AddCooldownToUser((long) Context.User.Id,
                            Cooldown.GamblingBet, timeNow.AddMinutes(
                                await _propertyService.GetPropertyValue(Property.CooldownCasinoBet)));
                        // добавляем пользователю статистку сделанных бросков
                        await _statisticService.AddStatisticToUser((long) Context.User.Id, Statistic.CasinoBet);
                        // проверяем выполнил ли пользователь достижение
                        await _achievementService.CheckAchievement((long) Context.User.Id, new[]
                        {
                            Achievement.Casino33Bet,
                            Achievement.Casino777Bet
                        });

                        var embed = new EmbedBuilder()
                            .WithDescription(cubeDropString);

                        await _discordEmbedService.SendEmbed(Context.User, embed);
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
