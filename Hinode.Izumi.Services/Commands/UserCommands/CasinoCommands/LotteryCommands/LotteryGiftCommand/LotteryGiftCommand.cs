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
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryGiftCommand
{
    [InjectableService]
    public class LotteryGiftCommand : ILotteryGiftCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;
        private readonly IEffectService _effectService;
        private readonly IInventoryService _inventoryService;
        private readonly IStatisticService _statisticService;
        private readonly IAchievementService _achievementService;
        private readonly IDiscordGuildService _discordGuildService;

        public LotteryGiftCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IPropertyService propertyService, ILocalizationService local, IUserService userService,
            IEffectService effectService, IInventoryService inventoryService, IStatisticService statisticService,
            IAchievementService achievementService, IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _propertyService = propertyService;
            _local = local;
            _userService = userService;
            _effectService = effectService;
            _inventoryService = inventoryService;
            _statisticService = statisticService;
            _achievementService = achievementService;
            _discordGuildService = discordGuildService;
        }

        public async Task Execute(SocketCommandContext context, string namePattern)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // находим пользователя которому нужно сделать подарок
            var target = await _userService.GetUser(namePattern);

            // проверяем не является ли цель этим же пользователем
            if (target.Id == (long) context.User.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftYourself.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"))));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);
                // получаем стоимость лотерейного билета
                var lotteryPrice = await _propertyService.GetPropertyValue(Property.LotteryPrice);
                // получаем стоимость отправки лотерейного билета в подарок
                var lotteryDeliveryPrice = await _propertyService.GetPropertyValue(Property.LotteryDeliveryPrice);
                // проверяем есть ли у цели эффект лотереи
                var targetHasLottery = await _effectService.CheckUserHasEffect(target.Id, Effect.Lottery);

                if (targetHasLottery)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftAlreadyHave.Parse(
                        emotes.GetEmoteOrBlank(target.Title.Emote()), target.Title.Localize(),
                        target.Name, emotes.GetEmoteOrBlank("LotteryTicket"))));
                }
                // проверяем хватает ли денег пользователю для оплаты подарка лотерейного билета
                else if (userCurrency.Amount < lotteryPrice + lotteryDeliveryPrice)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.LotteryGiftNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString()))));
                }
                else
                {
                    // отнимаем у пользователя деньги за подарок лотерейного билета
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(),
                        lotteryPrice + lotteryDeliveryPrice);
                    // добавляем цели эффект лотереи
                    await _effectService.AddEffectToUser(target.Id, EffectCategory.Lottery, Effect.Lottery);
                    // побавляем пользователю статистику подаренных лотерейных билетов
                    await _statisticService.AddStatisticToUser((long) context.User.Id, Statistic.CasinoLotteryGift);
                    // проверяем не выполнил ли пользователь достижение
                    await _achievementService.CheckAchievement(
                        (long) context.User.Id, Achievement.Casino20LotteryGift);

                    var embed = new EmbedBuilder()
                        // подтвеждаем успешную отправку подарка лотерейного билета
                        .WithDescription(IzumiReplyMessage.LotteryGiftSuccess.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(target.Title.Emote()),
                            target.Title.Localize(), target.Name));

                    await _discordEmbedService.SendEmbed(context.User, embed);

                    // получаем пользователя который делает подарок
                    var user = await _userService.GetUser((long) context.User.Id);
                    var embedPm = new EmbedBuilder()
                        // оповещаем цель о том, что ей подарили лотерейный билет
                        .WithDescription(IzumiReplyMessage.LotteryGiftSuccessPm.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(user.Title.Emote()),
                            user.Title.Localize(), user.Name));

                    await _discordEmbedService.SendEmbed(
                        await _discordGuildService.GetSocketUser(target.Id), embedPm);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
