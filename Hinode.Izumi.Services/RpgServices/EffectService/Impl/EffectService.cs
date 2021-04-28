using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.EffectService.Models;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.EffectService.Impl
{
    [InjectableService]
    public class EffectService : IEffectService
    {
        private readonly IConnectionManager _con;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IStatisticService _statisticService;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ILocalizationService _local;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IPropertyService _propertyService;

        public EffectService(IConnectionManager con, IEmoteService emoteService, IInventoryService inventoryService,
            IStatisticService statisticService, IDiscordEmbedService discordEmbedService, ILocalizationService local,
            IDiscordGuildService discordGuildService, IPropertyService propertyService)
        {
            _con = con;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _statisticService = statisticService;
            _discordEmbedService = discordEmbedService;
            _local = local;
            _discordGuildService = discordGuildService;
            _propertyService = propertyService;
        }

        public async Task<UserEffectModel[]> GetUserEffect(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserEffectModel>(@"
                    select * from user_effects
                    where user_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<UserModel[]> GetUsersWithEffect(Effect effect) =>
            (await _con.GetConnection()
                .QueryAsync<UserModel>(@"
                    select u.* from user_effects as ue
                        inner join users u
                            on u.id = ue.user_id
                    where ue.effect = @effect",
                    new {effect}))
            .ToArray();

        public async Task<bool> CheckUserHasEffect(long userId, Effect effect) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_effects
                    where user_id = @userId
                      and effect = @effect",
                    new {userId, effect});

        public async Task AddEffectToUser(long userId, EffectCategory category, Effect effect,
            DateTime? expiration = null)
        {
            // добавляем эффект пользователю
            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    insert into user_effects(user_id, category, effect, expiration)
                    values (@userId, @category, @effect, @expiration)
                    on conflict (user_id, effect) do nothing",
                    new {userId, category, effect, expiration});

            // если эффект это лотерея, необходимо проверить нужно ли начать лотерею
            if (category == EffectCategory.Lottery) await CheckLottery();
        }

        public async Task RemoveEffectFromUser(long userId, Effect effect) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_effects
                    where user_id = @userId
                      and effect = @effect",
                    new {userId, effect});

        /// <summary>
        /// Проверяет необходимо ли начать лотерею.
        /// </summary>
        private async Task CheckLottery()
        {
            // получаем количество пользователей лотереи
            var lotteryUsers = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from user_effects
                    where effect = @lotteryEffect",
                    new {lotteryEffect = Effect.Lottery});
            // получаем количество пользователей которое необходимое для проведения лотереи
            var lotteryRequireUsers = await _propertyService.GetPropertyValue(Property.LotteryRequireUsers);

            // если количество пользователей достаточное - начинаем лотерею
            if (lotteryUsers >= lotteryRequireUsers) await StartLottery();
        }

        /// <summary>
        /// Выбирает случайного пользователя, который побеждает в лотерее.
        /// </summary>
        private async Task StartLottery()
        {
            var winner = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(
                    // получаем случайного пользователя, который победил в лотерее
                    @"
                    select * from users
                    where id = (
                        select id from user_effects
                        where effect = @lotteryEffect
                        order by random()
                        limit 1
                        );"
                    // снимаем эффект лотереи со всех пользоваталей
                    + @"
                    delete from user_effects where effect = @lotteryEffect",
                    new {lotteryEffect = Effect.Lottery});

            // получаем количество валюты за победу в лотерею
            var lotteryAward = await _propertyService.GetPropertyValue(Property.LotteryAward);

            // добавляем победителю награду
            await _inventoryService.AddItemToUser(
                winner.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), lotteryAward);
            // добавляем победителю статистику
            await _statisticService.AddStatisticToUser(winner.Id, Statistic.CasinoLotteryWin);

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            var embedPm = new EmbedBuilder()
                // подтверждаем победу в лотерее
                .WithDescription(IzumiReplyMessage.LotteryWinnerPm.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    lotteryAward, _local.Localize(Currency.Ien.ToString(), lotteryAward)));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(winner.Id), embedPm);

            // TODO global message
        }
    }
}
