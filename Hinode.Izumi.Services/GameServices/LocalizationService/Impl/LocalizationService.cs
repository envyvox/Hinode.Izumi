using System;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.LocalizationService.Records;

namespace Hinode.Izumi.Services.GameServices.LocalizationService.Impl
{
    [InjectableService]
    public class LocalizationService : ILocalizationService
    {
        private readonly IConnectionManager _con;

        public LocalizationService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<LocalizationRecord> GetLocalizationByLocalizedWord(LocalizationCategory category,
            string localizedWord)
        {
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationRecord>(@"
                    select * from localizations
                    where category = @category
                      and (single ilike '%'||@localizedWord||'%'
                               or double ilike '%'||@localizedWord||'%'
                               or multiply ilike '%'||@localizedWord||'%')
                    order by id
                    limit 1",
                    new {category, localizedWord});

            if (res is null)
                await Task.FromException(new Exception(category switch
                {
                    // текст ошибки зависит от категории локализации
                    LocalizationCategory.Gathering => IzumiNullableMessage.LocalizationByLocalizedWordCrafting.Parse(),
                    LocalizationCategory.Product => IzumiNullableMessage.LocalizationByLocalizedWordProduct.Parse(),
                    LocalizationCategory.Crafting => IzumiNullableMessage.LocalizationByLocalizedWordCrafting.Parse(),
                    LocalizationCategory.Alcohol => IzumiNullableMessage.LocalizationByLocalizedWordAlcohol.Parse(),
                    LocalizationCategory.Drink => IzumiNullableMessage.LocalizationByLocalizedWordDrink.Parse(),
                    LocalizationCategory.Seed => IzumiNullableMessage.LocalizationByLocalizedWordSeed.Parse(),
                    LocalizationCategory.Crop => IzumiNullableMessage.LocalizationByLocalizedWordCrop.Parse(),
                    LocalizationCategory.Fish => IzumiNullableMessage.LocalizationByLocalizedWordFish.Parse(),
                    LocalizationCategory.Food => IzumiNullableMessage.LocalizationByLocalizedWordFood.Parse(),
                    LocalizationCategory.Currency => IzumiNullableMessage.LocalizationByLocalizedWordCurrency.Parse(),
                    LocalizationCategory.Bar => IzumiNullableMessage.LocalizationByLocalizedWordBar.Parse(),
                    LocalizationCategory.Box => IzumiNullableMessage.LocalizationByLocalizedWordBox.Parse(),
                    LocalizationCategory.Points => IzumiNullableMessage.LocalizationByLocalizedWordPoint.Parse(),
                    LocalizationCategory.Seafood => IzumiNullableMessage.LocalizationByLocalizedWordSeafood.Parse(),
                    _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                }));

            return res;
        }

        public async Task<LocalizationRecord> GetLocalizationByLocalizedWord(InventoryCategory category,
            string localizedWord) =>
            await GetLocalizationByLocalizedWord(category switch
            {
                InventoryCategory.Currency => LocalizationCategory.Currency,
                InventoryCategory.Gathering => LocalizationCategory.Gathering,
                InventoryCategory.Product => LocalizationCategory.Product,
                InventoryCategory.Crafting => LocalizationCategory.Crafting,
                InventoryCategory.Alcohol => LocalizationCategory.Alcohol,
                InventoryCategory.Drink => LocalizationCategory.Drink,
                InventoryCategory.Seed => LocalizationCategory.Seed,
                InventoryCategory.Crop => LocalizationCategory.Crop,
                InventoryCategory.Fish => LocalizationCategory.Fish,
                InventoryCategory.Food => LocalizationCategory.Food,
                InventoryCategory.Box => LocalizationCategory.Box,
                InventoryCategory.Seafood => LocalizationCategory.Seafood,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, localizedWord);

        public async Task<LocalizationRecord> GetLocalizationByLocalizedWord(MarketCategory category,
            string localizedWord) =>
            await GetLocalizationByLocalizedWord(category switch
            {
                MarketCategory.Gathering => LocalizationCategory.Gathering,
                MarketCategory.Crafting => LocalizationCategory.Crafting,
                MarketCategory.Alcohol => LocalizationCategory.Alcohol,
                MarketCategory.Drink => LocalizationCategory.Drink,
                MarketCategory.Food => LocalizationCategory.Food,
                MarketCategory.Crop => LocalizationCategory.Crop,
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, localizedWord);

        public string Localize(string keyword, long amount = 1)
        {
            var localization = GetLocalizationByKeyword(keyword).Result;
            return Localize(localization, amount);
        }

        public string Localize(LocalizationCategory category, long itemId, long amount = 1)
        {
            var localization = GetLocalizationByItemId(category, itemId).Result;
            return Localize(localization, amount);
        }

        public string Localize(MarketCategory requestCategory, long itemId, long amount = 1)
        {
            var localization = GetLocalizationByItemId(requestCategory switch
            {
                MarketCategory.Gathering => LocalizationCategory.Gathering,
                MarketCategory.Crafting => LocalizationCategory.Crafting,
                MarketCategory.Alcohol => LocalizationCategory.Alcohol,
                MarketCategory.Drink => LocalizationCategory.Drink,
                MarketCategory.Food => LocalizationCategory.Food,
                MarketCategory.Crop => LocalizationCategory.Crop,
                _ => throw new ArgumentOutOfRangeException(nameof(requestCategory), requestCategory, null)
            }, itemId).Result;
            return Localize(localization, amount);
        }

        private async Task<LocalizationRecord> GetLocalizationByKeyword(string keyword)
        {
            var localization = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationRecord>(@"
                    select * from localizations
                    where name = @keyword",
                    new {keyword});

            if (localization is null)
                await Task.FromException(new Exception(IzumiNullableMessage.LocalizationByKeyword.Parse()));

            return localization;
        }

        private async Task<LocalizationRecord> GetLocalizationByItemId(LocalizationCategory category, long itemId)
        {
            var localization = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationRecord>(@"
                    select * from localizations
                    where category = @category
                      and item_id = @itemId",
                    new {category, itemId});

            if (localization is null)
                await Task.FromException(new Exception(IzumiNullableMessage.LocalizationByKeyword.Parse()));

            return localization;
        }

        private static string Localize(LocalizationRecord localization, long amount)
        {
            var n = Math.Abs(amount);

            n %= 100;
            if (n >= 5 && n <= 20) return localization.Multiply;

            n %= 10;
            if (n == 1) return localization.Single;
            if (n >= 2 && n <= 4) return localization.Double;

            return localization.Multiply;
        }
    }
}
