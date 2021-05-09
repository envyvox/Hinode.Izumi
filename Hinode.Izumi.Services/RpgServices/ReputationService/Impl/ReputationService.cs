using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CardService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.ReputationService.Models;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.RpgServices.ReputationService.Impl
{
    [InjectableService]
    public class ReputationService : IReputationService
    {
        private readonly IConnectionManager _con;
        private readonly IPropertyService _propertyService;
        private readonly IInventoryService _inventoryService;
        private readonly IFoodService _foodService;
        private readonly ICardService _cardService;
        private readonly IUserService _userService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ILocalizationService _local;
        private readonly IEmoteService _emoteService;

        public ReputationService(IConnectionManager con, IPropertyService propertyService,
            IInventoryService inventoryService, IFoodService foodService, ICardService cardService,
            IUserService userService, IDiscordGuildService discordGuildService, IEmoteService emoteService,
            IDiscordEmbedService discordEmbedService, ILocalizationService local)
        {
            _con = con;
            _propertyService = propertyService;
            _inventoryService = inventoryService;
            _foodService = foodService;
            _cardService = cardService;
            _userService = userService;
            _discordGuildService = discordGuildService;
            _discordEmbedService = discordEmbedService;
            _local = local;
            _emoteService = emoteService;
        }

        public async Task<UserReputationModel> GetUserReputation(long userId, Reputation reputation) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserReputationModel>(@"
                    select * from user_reputations
                    where user_id = @userId
                      and reputation = @reputation",
                    new {userId, reputation}) ??
            new UserReputationModel {UserId = userId, Reputation = reputation, Amount = 0};

        public async Task<Dictionary<Reputation, UserReputationModel>> GetUserReputation(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<UserReputationModel>(@"
                    select * from user_reputations
                    where user_id = @userId",
                    new {userId}))
            .ToDictionary(x => x.Reputation);

        public async Task AddReputationToUser(long userId, Reputation reputation, long amount)
        {
            // добавляем репутацию пользователю
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (@userId, @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.amount + @amount,
                            updated_at = now()",
                    new {userId, reputation, amount});
            // проверяем нужно ли выдать ему награды
            await CheckReputationRewards(userId, reputation);
        }

        public async Task AddReputationToUser(long[] usersId, Reputation reputation, long amount)
        {
            // добавляем репутацию пользователям
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (unnest(array[@usersId]), @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.reputation + @reputation,
                            updated_at = now()",
                    new {usersId, reputation, amount});
            // для каждого из них проверяем нужно ли выдать награды
            foreach (var userId in usersId) await CheckReputationRewards(userId, reputation);
        }


        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        public Reputation GetReputationByLocation(Location location) => location switch
        {
            Location.Capital => Reputation.Capital,
            Location.Garden => Reputation.Garden,
            Location.Seaport => Reputation.Seaport,
            Location.Castle => Reputation.Castle,
            Location.Village => Reputation.Village,
            _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
        };

        /// <summary>
        /// Проверяет получение пользователем награды за репутацию.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="reputation">Репутация.</param>
        private async Task CheckReputationRewards(long userId, Reputation reputation)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем репутацию пользователя
            var userReputation = await GetUserReputation(userId, reputation);

            // проверяем получение награды в зависимости от количества репутации
            switch (userReputation.Amount)
            {
                case >= 10000:

                    // проверяем получал ли пользователь награду
                    var check10000RepReward = await CheckUserReputationReward(userId, reputation, 10000);
                    // если пользователь уже получал награду - игнорируем
                    if (check10000RepReward) return;

                    // получаем титул который нужно выдать
                    var title = (Title) await _propertyService.GetPropertyValue(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalTitleNumber,
                        Reputation.Garden => Property.ReputationGardenTitleNumber,
                        Reputation.Seaport => Property.ReputationSeaportTitleNumber,
                        Reputation.Castle => Property.ReputationCastleTitleNumber,
                        Reputation.Village => Property.ReputationVillageTitleNumber,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    });

                    // выдаем пользователю титул
                    await _userService.AddTitleToUser(userId, title);
                    // записываем в базу выдачу награды за репутацию
                    await AddUserReputationReward(userId, reputation, 10000);
                    // оповещаем пользователя о получении награды
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(10000)), 10000, reputation.Location().Localize(true),
                        "титул", emotes.GetEmoteOrBlank(title.Emote()), title.Localize()));

                    break;
                case >= 5000:

                    // проверяем получал ли пользователь награду
                    var check5000RepReward = await CheckUserReputationReward(userId, reputation, 5000);
                    // если пользователь уже получал награду - игнорируем
                    if (check5000RepReward) return;

                    // получаем id карточки которую нужно выдать
                    var cardId = await _propertyService.GetPropertyValue(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalCardId,
                        Reputation.Garden => Property.ReputationGardenCardId,
                        Reputation.Seaport => Property.ReputationSeaportCardId,
                        Reputation.Castle => Property.ReputationCastleCardId,
                        Reputation.Village => Property.ReputationVillageCardId,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    });
                    // получаем карточку
                    var card = await _cardService.GetCard(cardId);

                    // выдаем карточку пользователю
                    await _cardService.AddCardToUser(userId, card.Id);
                    // записываем в базу выдачу награды за репутацию
                    await AddUserReputationReward(userId, reputation, 5000);
                    // оповещаем пользователя о получении награды
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(5000)), 5000, reputation.Location().Localize(true),
                        "", card.Rarity.Localize(true), $"[{card.Name}]({card.Url})"));

                    break;
                case >= 2000:

                    // проверяем получал ли пользователь награду
                    var check2000RepReward = await CheckUserReputationReward(userId, reputation, 2000);
                    // если пользователь уже получал награду - игнорируем
                    if (check2000RepReward) return;

                    // получаем количество жемчуга которое нужно выдать
                    var pearlAmount = await _propertyService.GetPropertyValue(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalPearlAmount,
                        Reputation.Garden => Property.ReputationGardenPearlAmount,
                        Reputation.Seaport => Property.ReputationSeaportPearlAmount,
                        Reputation.Castle => Property.ReputationCastlePearlAmount,
                        Reputation.Village => Property.ReputationVillagePearlAmount,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    });

                    // выдаем жемгуч пользователю
                    await _inventoryService.AddItemToUser(
                        userId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), pearlAmount);
                    // записываем в базу выдачу награды за репутацию
                    await AddUserReputationReward(userId, reputation, 2000);
                    // оповещаем пользователя о получении награды
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(2000)), 2000, reputation.Location().Localize(true),
                        emotes.GetEmoteOrBlank(Currency.Pearl.ToString()), pearlAmount,
                        _local.Localize(Currency.Pearl.ToString(), pearlAmount)));

                    break;
                case >= 1000:

                    // проверяем получал ли пользователь награду
                    var check1000RepReward = await CheckUserReputationReward(userId, reputation, 1000);
                    // если пользователь уже получал награду - игнорируем
                    if (check1000RepReward) return;

                    // получаем id блюда, рецепт которого нужно выдать
                    var foodId = await _propertyService.GetPropertyValue(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalFoodId,
                        Reputation.Garden => Property.ReputationGardenFoodId,
                        Reputation.Seaport => Property.ReputationSeaportFoodId,
                        Reputation.Castle => Property.ReputationCastleFoodId,
                        Reputation.Village => Property.ReputationVillageFoodId,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    });
                    // получаем блюдо
                    var food = await _foodService.GetFood(foodId);

                    // выдаем рецепт пользователю
                    await _foodService.AddRecipeToUser(userId, food.Id);
                    // записываем в базу выдачу награды за репутацию
                    await AddUserReputationReward(userId, reputation, 1000);
                    // оповещаем пользователя о получении награды
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(1000)), 1000, reputation.Location().Localize(true),
                        "", emotes.GetEmoteOrBlank("Recipe"), _local.Localize(LocalizationCategory.Food, food.Id)));

                    break;
                case >= 500:

                    // проверяем получал ли пользователь награду
                    var check500RepReward = await CheckUserReputationReward(userId, reputation, 500);
                    // если пользователь уже получал награду - игнорируем
                    if (check500RepReward) return;

                    // получаем коробку в зависимости от репутации
                    var box = reputation switch
                    {
                        Reputation.Capital => Box.Capital,
                        Reputation.Garden => Box.Garden,
                        Reputation.Seaport => Box.Seaport,
                        Reputation.Castle => Box.Castle,
                        Reputation.Village => Box.Village,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    };
                    // получаем количество коробок которое нужно выдать
                    var boxAmount = await _propertyService.GetPropertyValue(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalBoxAmount,
                        Reputation.Garden => Property.ReputationGardenBoxAmount,
                        Reputation.Seaport => Property.ReputationSeaportBoxAmount,
                        Reputation.Castle => Property.ReputationCastleBoxAmount,
                        Reputation.Village => Property.ReputationVillageBoxAmount,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    });

                    // выдаем пользователю коробки
                    await _inventoryService.AddItemToUser(
                        userId, InventoryCategory.Box, box.GetHashCode(), boxAmount);
                    // записываем в базу выдачу награды за репутацию
                    await AddUserReputationReward(userId, reputation, 500);
                    // оповещаем пользователя о получении награды
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(500)), 500, reputation.Location().Localize(true),
                        emotes.GetEmoteOrBlank(box.Emote()), boxAmount, _local.Localize(box.ToString(), boxAmount)));

                    break;
            }
        }

        /// <summary>
        /// Проверяет получение пользователем награды за репутацию.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="reputation">Репутация.</param>
        /// <param name="amount">Количество репутации (брекет).</param>
        /// <returns>True если получал, false если нет.</returns>
        private async Task<bool> CheckUserReputationReward(long userId, Reputation reputation, long amount) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_reputation_rewards
                    where user_id = @userId
                      and reputation = @reputation
                      and amount = @amount",
                    new {userId, reputation, amount});

        /// <summary>
        /// Записывает в базу получение пользователем награды за репутацию.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="reputation">Репутация.</param>
        /// <param name="amount">Количество репутации (брекет).</param>
        private async Task AddUserReputationReward(long userId, Reputation reputation, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputation_rewards(user_id, reputation, amount)
                    values (@userId, @reputation, @amount)",
                    new {userId, reputation, amount});

        /// <summary>
        /// Отправляет пользователю оповещение о получении награды за репутацию.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="message">Сообщение.</param>
        private async Task NotifyUserReputationReward(long userId, string message) =>
            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId),
                new EmbedBuilder()
                    .WithAuthor(IzumiReplyMessage.ReputationRewardAuthor.Parse())
                    .WithDescription(message));
    }
}
