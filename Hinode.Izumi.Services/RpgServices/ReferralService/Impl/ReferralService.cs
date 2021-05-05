using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.ReferralService.Impl
{
    [InjectableService]
    public class ReferralService : IReferralService
    {
        private readonly IConnectionManager _con;
        private readonly IInventoryService _inventoryService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;

        public ReferralService(IConnectionManager con, IInventoryService inventoryService, IUserService userService,
            IEmoteService emoteService, ILocalizationService local, IDiscordEmbedService discordEmbedService,
            IDiscordGuildService discordGuildService)
        {
            _con = con;
            _inventoryService = inventoryService;
            _userService = userService;
            _emoteService = emoteService;
            _local = local;
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
        }

        public async Task<UserModel> GetUserReferrer(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    select u.* from user_referrers as ur
                        inner join users u
                            on u.id = ur.referrer_id
                    where ur.user_id = @userId",
                    new {userId});

        public async Task<UserModel[]> GetUserReferrals(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserModel>(@"
                    select * from user_referrers as ur
                        inner join users u
                            on u.id = ur.user_id
                    where ur.referrer_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<bool> CheckUserHasReferrer(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_referrers
                    where user_id = @userId",
                    new {userId});

        public async Task<long> GetUserReferralCount(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from user_referrers
                    where referrer_id = @userId",
                    new {userId});

        public async Task AddUserReferrer(long userId, long referrerId)
        {
            // добавляем пользователю реферера
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_referrers(user_id, referrer_id)
                    values (@userId, @referrerId)",
                    new {userId, referrerId});

            // выдаем пользователю коробку столицы
            await _inventoryService.AddItemToUser(userId, InventoryCategory.Box, Box.Capital.GetHashCode());
            // выдаем награды реферреру
            await AddRewardsToReferrer(userId, referrerId);
        }

        private async Task AddRewardsToReferrer(long userId, long referrerId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем количество приглашенных пользователей этим реферерром
            var referralCount = await GetUserReferralCount(referrerId);

            // записываем награду в зависимости от количества приглашенных пользователей
            var rewardString = string.Empty;
            switch (referralCount)
            {
                case 1 or 2:

                    // добавляем коробки реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode());
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} {_local.Localize(Box.Capital.ToString())}";

                    break;
                case 3 or 4:

                    // добавляем коробки реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode(), 2);
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} 2 {_local.Localize(Box.Capital.ToString(), 2)}";

                    break;
                case 5:

                    // добавляем коробки реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode(), 3);
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} 3 {_local.Localize(Box.Capital.ToString(), 3)}";

                    break;
                case 6 or 7 or 8 or 9:

                    // добавляем донат валюту реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 10);
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 10 {_local.Localize(Currency.Pearl.ToString(), 10)}";

                    break;
                case 10:

                    // добавляем донат валюту реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 10);
                    // добавляем титул реферреру
                    await _userService.AddTitleToUser(referrerId, Title.Yatagarasu);
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 10 {_local.Localize(Currency.Pearl.ToString(), 10)}, " +
                        $"титул {emotes.GetEmoteOrBlank(Title.Yatagarasu.Emote())} {Title.Yatagarasu.Localize()}";

                    break;
                case > 10:

                    // добавляем донат валюту реферреру
                    await _inventoryService.AddItemToUser(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 15);
                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 15 {_local.Localize(Currency.Pearl.ToString(), 15)}";

                    break;
            }

            // получаем пользователя который указал реферрера
            var user = await _userService.GetUser(userId);
            var embedPm = new EmbedBuilder()
                // оповещаем цель что ее указали как пригласившего пользователя
                .WithDescription(IzumiReplyMessage.ReferralSetNotifyPm.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name))
                // бонус реферальной системы
                .AddField(IzumiReplyMessage.ReferralRewardFieldName.Parse(), rewardString);

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(referrerId), embedPm);
        }
    }
}
