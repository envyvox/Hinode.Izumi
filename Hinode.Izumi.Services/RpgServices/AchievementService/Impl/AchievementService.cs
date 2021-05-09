using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AchievementService.Models;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CollectionService;
using Hinode.Izumi.Services.RpgServices.CollectionService.Models;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FishService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.StatisticService;
using Hinode.Izumi.Services.RpgServices.StatisticService.Models;
using Hinode.Izumi.Services.RpgServices.UserService;
using Microsoft.Extensions.Caching.Memory;
using Achievement = Hinode.Izumi.Data.Enums.AchievementEnums.Achievement;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.AchievementService.Impl
{
    [InjectableService]
    public class AchievementService : IAchievementService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;
        private readonly IStatisticService _statisticService;
        private readonly ICollectionService _collectionService;
        private readonly ICropService _cropService;
        private readonly IFishService _fishService;
        private readonly IFoodService _foodService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;

        private const string AchievementIdKey = "achievement_id_{0}";
        private const string AchievementTypeKey = "achievement_type_{0}";
        private const string UserAchievementKey = "user_{0}_achievement_{1}";

        public AchievementService(IConnectionManager con, IMemoryCache cache, IDiscordEmbedService discordEmbedService,
            IDiscordGuildService discordGuildService, IEmoteService emoteService, IInventoryService inventoryService,
            ILocalizationService local, IUserService userService, IStatisticService statisticService,
            ICollectionService collectionService, ICropService cropService, IFishService fishService,
            IFoodService foodService, IAlcoholService alcoholService, IDrinkService drinkService,
            IGatheringService gatheringService, ICraftingService craftingService)
        {
            _con = con;
            _cache = cache;
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _local = local;
            _userService = userService;
            _statisticService = statisticService;
            _collectionService = collectionService;
            _cropService = cropService;
            _fishService = fishService;
            _foodService = foodService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
        }

        public async Task<AchievementModel> GetAchievement(long id)
        {
            // проверяем достижение в кэше
            if (_cache.TryGetValue(string.Format(AchievementIdKey, id), out AchievementModel achievement))
                return achievement;

            // получаем достижение из базы
            achievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementModel>(@"
                    select * from achievements
                    where id = @id",
                    new {id});

            // добавляем достижение в кэш
            _cache.Set(string.Format(AchievementIdKey, id), achievement, CacheExtensions.DefaultCacheOptions);

            // возвращаем достижение
            return achievement;
        }

        public async Task<AchievementModel> GetAchievement(Achievement type)
        {
            if (_cache.TryGetValue(string.Format(AchievementTypeKey, type), out AchievementModel achievement))
                return achievement;

            // получаем достижение из базы
            achievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AchievementModel>(@"
                    select * from achievements
                    where type = @type",
                    new {type});

            // добавляем достижение в кэш
            _cache.Set(string.Format(AchievementTypeKey, type), achievement, CacheExtensions.DefaultCacheOptions);

            // возвращаем достижение
            return achievement;
        }

        public async Task<UserAchievementModel[]> GetUserAchievement(long userId, AchievementCategory category) =>
            (await _con.GetConnection()
                .QueryAsync<UserAchievementModel>(@"
                    select ua.* from user_achievements as ua
                        inner join achievements a
                            on a.id = ua.achievement_id
                    where ua.user_id = @userId
                      and a.category = @category",
                    new {userId, category}))
            .ToArray();

        public async Task CheckAchievement(long userId, Achievement type)
        {
            // получаем достижение
            var achievement = await GetAchievement(type);
            // проверяем есть ли у пользователя это достижение
            var hasAchievement = await CheckAchievementInUser(userId, achievement.Id);
            // если есть - пропускаем
            if (hasAchievement) return;

            UserStatisticModel userStatistic;
            UserCollectionModel[] userCollection;
            int collectionLength;

            // для каждого достижения необходимо сделать свое действие
            switch (achievement.Type)
            {
                // достижения которые выполняются в один шаг
                case Achievement.FirstMessage:
                case Achievement.FirstTransit:
                case Achievement.FirstFish:
                case Achievement.FirstGatheringResource:
                case Achievement.FirstPlant:
                case Achievement.FirstCraftResource:
                case Achievement.FirstCook:
                case Achievement.FirstBet:
                case Achievement.FirstJackPot:
                case Achievement.FirstLotteryTicket:
                case Achievement.FirstMarketDeal:
                case Achievement.FirstContract:
                case Achievement.FirstGiftSent:
                case Achievement.FirstGiftReceived:
                case Achievement.CatchEpicFish:
                case Achievement.CatchMythicalFish:
                case Achievement.CatchLegendaryFish:
                case Achievement.CatchKoi:
                case Achievement.FirstCraftAlcohol:

                    await AddAchievementToUser(userId, achievement.Type);

                    break;

                // достижения которые связанны со статистикой
                case Achievement.Catch50Fish:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Fishing);

                    if (userStatistic?.Amount >= 50)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Catch300Fish:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Fishing);

                    if (userStatistic?.Amount >= 300)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Plant25Seed:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.SeedPlanted);

                    if (userStatistic?.Amount >= 25)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Plant150Seed:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.SeedPlanted);

                    if (userStatistic?.Amount >= 150)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Craft30Resource:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CraftingResource);

                    if (userStatistic?.Amount >= 30)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Craft250Resource:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CraftingResource);

                    if (userStatistic?.Amount >= 250)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Cook20Food:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Cooking);

                    if (userStatistic?.Amount >= 20)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Cook130Food:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Cooking);

                    if (userStatistic?.Amount >= 130)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Gather40Resources:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Gathering);

                    if (userStatistic?.Amount >= 40)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Gather250Resources:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.Gathering);

                    if (userStatistic?.Amount >= 250)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Craft10Alcohol:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CraftingAlcohol);

                    if (userStatistic?.Amount >= 10)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Craft80Alcohol:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CraftingAlcohol);

                    if (userStatistic?.Amount >= 80)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Collect50Crop:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CropHarvested);

                    if (userStatistic?.Amount >= 50)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Collect300Crop:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CropHarvested);

                    if (userStatistic?.Amount >= 300)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Casino33Bet:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CasinoBet);

                    if (userStatistic?.Amount >= 33)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Casino777Bet:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CasinoBet);

                    if (userStatistic?.Amount >= 777)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Casino22LotteryBuy:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CasinoLotteryBuy);

                    if (userStatistic?.Amount >= 22)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Casino99LotteryBuy:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CasinoLotteryBuy);

                    if (userStatistic?.Amount >= 99)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Casino20LotteryGift:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.CasinoLotteryGift);

                    if (userStatistic?.Amount >= 20)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Market100Sell:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.MarketSell);

                    if (userStatistic?.Amount >= 100)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Market666Sell:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.MarketSell);

                    if (userStatistic?.Amount >= 666)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Market50Buy:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.MarketBuy);

                    if (userStatistic?.Amount >= 50)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.Market333Buy:

                    userStatistic = await _statisticService.GetUserStatistic(userId, Statistic.MarketBuy);

                    if (userStatistic?.Amount >= 333)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;

                // достижения которые связанны с коллекцией
                case Achievement.CompleteCollectionGathering:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Gathering);
                    collectionLength = (await _gatheringService.GetAllGatherings()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.CompleteCollectionCrafting:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Crafting);
                    collectionLength = (await _craftingService.GetAllCraftings()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.CompleteCollectionAlcohol:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Alcohol);
                    collectionLength = (await _alcoholService.GetAllAlcohol()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.CompleteCollectionCrop:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Crop);
                    collectionLength = (await _cropService.GetAllCrops()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.CompleteCollectionFish:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Fish);
                    collectionLength = (await _fishService.GetAllFish()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                case Achievement.CompleteCollectionFood:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Food);
                    collectionLength = (await _foodService.GetAllFood()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;

                case Achievement.CompleteCollectionDrink:

                    userCollection = await _collectionService.GetUserCollection(userId, CollectionCategory.Drink);
                    collectionLength = (await _drinkService.GetAllDrinks()).Length;

                    if (userCollection.Length >= collectionLength)
                        await AddAchievementToUser(userId, achievement.Type);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task CheckAchievement(IEnumerable<long> usersId, Achievement achievement)
        {
            foreach (var userId in usersId)
            {
                await CheckAchievement(userId, achievement);
            }
        }

        public async Task CheckAchievement(long userId, IEnumerable<Achievement> achievements)
        {
            foreach (var achievement in achievements)
            {
                await CheckAchievement(userId, achievement);
            }
        }

        public async Task CheckAchievement(IEnumerable<long> usersId, Achievement[] achievements)
        {
            foreach (var userId in usersId)
            {
                foreach (var achievement in achievements)
                {
                    await CheckAchievement(userId, achievement);
                }
            }
        }

        private async Task<bool> CheckAchievementInUser(long userId, long achievementId)
        {
            // проверяем ответ в кэше
            if (_cache.TryGetValue(string.Format(UserAchievementKey, userId, achievementId), out bool hasAchievement))
                return hasAchievement;

            // проверяем достижение пользователя в базе
            hasAchievement = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_achievements
                    where user_id = @userId
                      and achievement_id = @achievementId",
                    new {userId, achievementId});

            // добавляем ответ в кэш
            _cache.Set(string.Format(UserAchievementKey, userId, achievementId), hasAchievement,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем ответ
            return hasAchievement;
        }

        private async Task AddAchievementToUser(long userId, Achievement type)
        {
            // получаем достижение
            var achievement = await GetAchievement(type);

            // добавляем пользователю достижение
            await _con
                .GetConnection()
                .ExecuteAsync(@"
                    insert into user_achievements(user_id, achievement_id)
                    values (@userId, @achievementId)
                    on conflict (user_id, achievement_id) do nothing",
                    new {userId, achievementId = achievement.Id});

            // добавляем достижение пользователя в кэш
            _cache.Set(string.Format(UserAchievementKey, userId, achievement.Id), true,
                CacheExtensions.DefaultCacheOptions);

            // выдаем пользователю награду за выполнение достижения в зависимости от типа награды
            switch (achievement.Reward)
            {
                case AchievementReward.Ien:

                    await _inventoryService.AddItemToUser(
                        userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), achievement.Number);

                    break;
                case AchievementReward.Title:

                    await _userService.AddTitleToUser(userId, (Title) achievement.Number);

                    break;
                case AchievementReward.Pearl:

                    await _inventoryService.AddItemToUser(
                        userId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), achievement.Number);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            var embed = new EmbedBuilder()
                // подтверждаем выполнение достижения
                .WithDescription(IzumiReplyMessage.AchievementAdded.Parse(
                    emotes.GetEmoteOrBlank("Achievement"), achievement.Type.Localize(), achievement.Category.Localize(),
                    achievement.Reward switch
                    {
                        AchievementReward.Ien =>
                            $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {achievement.Number} {_local.Localize(Currency.Ien.ToString(), achievement.Number)}",

                        AchievementReward.Title =>
                            $"титул {emotes.GetEmoteOrBlank(((Title) achievement.Number).Emote())} {((Title) achievement.Number).Localize()}",

                        AchievementReward.Pearl =>
                            $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} {achievement.Number} {_local.Localize(Currency.Pearl.ToString(), achievement.Number)}",

                        _ => throw new ArgumentOutOfRangeException()
                    }));

            await _discordEmbedService.SendEmbed(
                await _discordGuildService.GetSocketUser(userId), embed);
        }
    }
}
