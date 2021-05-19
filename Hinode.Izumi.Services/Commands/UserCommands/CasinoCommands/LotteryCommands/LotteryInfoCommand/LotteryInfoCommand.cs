using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.EffectService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.Commands.UserCommands.CasinoCommands.LotteryCommands.LotteryInfoCommand
{
    [InjectableService]
    public class LotteryInfoCommand : ILotteryInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IEffectService _effectService;
        private readonly ILocalizationService _local;
        private readonly IPropertyService _propertyService;

        public LotteryInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IEffectService effectService, ILocalizationService local, IPropertyService propertyService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _effectService = effectService;
            _local = local;
            _propertyService = propertyService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем список пользователь с эффектом лотереи
            var lotteryUsers = await _effectService.GetUsersWithEffect(Effect.Lottery);
            // заполняем список для вывода
            var lotteryUsersString = lotteryUsers.Aggregate(string.Empty, (current, user) => current +
                $"{emotes.GetEmoteOrBlank("List")} {emotes.GetEmoteOrBlank(user.Title.Emote())} {user.Title.Localize()} **{user.Name}**\n");

            // получаем стоимость лотерейного билета
            var lotteryPrice = await _propertyService.GetPropertyValue(Property.LotteryPrice);
            // получаем награду за победу в лотерее
            var lotteryAward = await _propertyService.GetPropertyValue(Property.LotteryAward);
            // получаем стоимость отправки лотерейного билета в подарок
            var lotteryDeliveryPrice = await _propertyService.GetPropertyValue(Property.LotteryDeliveryPrice);
            // получаем необходимое количество пользователей для лотереи
            var lotteryRequireUsers = await _propertyService.GetPropertyValue(Property.LotteryRequireUsers);

            var embed = new EmbedBuilder()
                // правила участия
                .AddField(IzumiReplyMessage.LotteryInfoRulesFieldName.Parse(),
                    IzumiReplyMessage.LotteryInfoRulesFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryPrice, _local.Localize(Currency.Ien.ToString(), lotteryPrice),
                        lotteryRequireUsers, lotteryAward, _local.Localize(Currency.Ien.ToString(), lotteryAward)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")

                // служба доставки
                .AddField(IzumiReplyMessage.LotteryGiftInfoFieldName.Parse(),
                    IzumiReplyMessage.LotteryGiftInfoFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        lotteryDeliveryPrice, _local.Localize(Currency.Ien.ToString(), lotteryDeliveryPrice)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")

                // список пользователей с лотереей
                .AddField(IzumiReplyMessage.LotteryInfoCurrentMembersFieldName.Parse(),
                    lotteryUsersString.Length > 0
                        ? lotteryUsersString
                        : IzumiReplyMessage.LotteryInfoCurrentMembersNull.Parse(
                            emotes.GetEmoteOrBlank("LotteryTicket")));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
