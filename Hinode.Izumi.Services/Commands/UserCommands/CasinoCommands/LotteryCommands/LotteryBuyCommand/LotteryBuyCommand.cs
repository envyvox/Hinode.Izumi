using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.StatisticService;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryBuyCommand
{
    [InjectableService]
    public class LotteryBuyCommand : ILotteryBuyCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IEffectService _effectService;
        private readonly ILocalizationService _local;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IPropertyService _propertyService;

        public LotteryBuyCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, IEffectService effectService, ILocalizationService local,
            IStatisticService statisticService, IAchievementService achievementService,
            IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _effectService = effectService;
            _local = local;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем валюту пользователя
            var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
            // получаем стоимость лотерейного билета
            var lotteryPrice = await _propertyService.GetPropertyValue(Property.LotteryPrice);
            // проверяем есть ли у пользователя эффект лотереи
            var hasLottery = await _effectService.CheckUserHasEffect((long) context.User.Id, Effect.Lottery);

            if (hasLottery)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryBuyAlready.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            // проверяем хватает ли пользователю денег на покупку лотерейного билета
            else if (userCurrency.Amount < lotteryPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryBuyNoCurrency.Parse(
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    _local.Localize(Currency.Ien.ToString()), emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            else
            {
                // забираем у пользователя деньги за лотерейный билет
                await _inventoryService.RemoveItemFromUser(
                    (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), lotteryPrice);
                // добавляем пользователю эффект лотереи
                await _effectService.AddEffectToUser((long) context.User.Id, EffectCategory.Lottery, Effect.Lottery);
                // добавляем пользователю статистику купленных лотерейных билетов
                await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.CasinoLotteryBuy);
                // проверяем не выполнил ли пользователь достижение
                await _achievementService.CheckAchievement((long) context.User.Id, new[]
                {
                    Achievement.FirstLotteryTicket,
                    Achievement.Casino22LotteryBuy,
                    Achievement.Casino99LotteryBuy
                });

                var embed = new EmbedBuilder()
                    // подверждаем успешную покупку лотерейного билета
                    .WithDescription(IzumiReplyMessage.LotteryBuySuccess.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryPrice, _local.Localize(Currency.Ien.ToString(), lotteryPrice)));

                await _discordEmbedService.SendEmbed(context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
