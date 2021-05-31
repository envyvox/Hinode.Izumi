using System;
using Microsoft.Extensions.Caching.Memory;

namespace Hinode.Izumi.Services.Extensions
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Настройки кэша с длительностью хранения по-умолчанию.
        /// </summary>
        public static readonly MemoryCacheEntryOptions DefaultCacheOptions =
            new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        public const string AchievementIdKey = "achievement_id_{0}";
        public const string AchievementTypeKey = "achievement_type_{0}";
        public const string UserAchievementKey = "user_{0}_achievement_{1}";
        public const string AlcoholKey = "alcohol_{0}";
        public const string BuildingIdKey = "building_id_{0}";
        public const string BuildingTypeKey = "building_type_{0}";
        public const string BuildingByProjectKey = "building_project_id_{0}";
        public const string CardKey = "card_{0}";
        public const string CertificateKey = "certificate_{0}";
        public const string ContractKey = "contract_{0}";
        public const string ContractLocationKey = "contract_location_{0}";
        public const string CraftingKey = "crafting_{0}";
        public const string CropByIdKey = "crop_{0}";
        public const string CropBySeedKey = "crop_with_seed_id_{0}";
        public const string DrinkKey = "drink_{0}";
        public const string FishKey = "fish_{0}";
        public const string FoodKey = "food_{0}";
        public const string GatheringKey = "gathering_{0}";
        public const string ImageKey = "image_{0}";
        public const string TransitKey = "transit_from_{0}_to_{1}";
        public const string TransitsLocationKey = "transits_location_{0}";
        public const string ProductKey = "product_{0}";
        public const string ProjectKey = "project_{0}";
        public const string PropertyKey = "property_{0}";
        public const string MasteryPropertyKey = "mastery_property_{0}";
        public const string MasteryXpPropertyKey = "mastery_xp_property_{0}";
        public const string GatheringPropertyKey = "gathering_{0}_property_{1}";
        public const string CraftingPropertyKey = "crafting_{0}_property_{1}";
        public const string AlcoholPropertyKey = "alcohol_{0}_property_{1}";
        public const string SeedKey = "seed_{0}";
        public const string SeedByNameKey = "seed_name_{0}";
        public const string UserWithIdCheckKey = "user_id_{0}_check";
        public const string UserWithNameCheckKey = "user_name_{0}_check";
        public const string EmotesKey = "emotes";
        public const string BannerKey = "banner_{0}";
        public const string UserHasPremium = "user_{0}_premium";
        public const string UserCommandColor = "user_{0}_command_color";
    }
}
