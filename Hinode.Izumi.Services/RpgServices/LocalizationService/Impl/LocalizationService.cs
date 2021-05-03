using System;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.LocalizationService.Models;

namespace Hinode.Izumi.Services.RpgServices.LocalizationService.Impl
{
    [InjectableService]
    public class LocalizationService : ILocalizationService
    {
        private readonly IConnectionManager _con;

        public LocalizationService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<LocalizationModel> GetLocalizationByLocalizedWord(LocalizationCategory category,
            string localizedWord)
        {
            // ищем локализацию по локализированному слову
            var res = await _con
                .GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationModel>(@"
                    select * from localizations
                    where category = @category
                      and (single ilike '%'||@localizedWord||'%'
                               or double ilike '%'||@localizedWord||'%'
                               or multiply ilike '%'||@localizedWord||'%')
                    order by id
                    limit 1",
                    new {category, localizedWord});

            // если локализации не нашлось - выводим ошибку
            if (res == null)
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
                    _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                }));

            // возвращаем локализацию
            return res;
        }

        public async Task<LocalizationModel> GetLocalizationByLocalizedWord(InventoryCategory category,
            string localizedWord) =>
            await GetLocalizationByLocalizedWord(category switch
            {
                // переопределяем категорию
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
                _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
            }, localizedWord);

        public async Task<LocalizationModel> GetLocalizationByLocalizedWord(MarketCategory category,
            string localizedWord) =>
            await GetLocalizationByLocalizedWord(category switch
            {
                // переопределяем категорию
                MarketCategory.Gathering => LocalizationCategory.Gathering,
                MarketCategory.Crafting => LocalizationCategory.Crafting,
                MarketCategory.Alcohol => LocalizationCategory.Alcohol,
                MarketCategory.Drink => LocalizationCategory.Drink,
                MarketCategory.Food => LocalizationCategory.Food,
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

        /// <summary>
        /// Возвращает локализацию по ключевому названию.
        /// </summary>
        /// <param name="keyword">Ключевое название.</param>
        /// <returns></returns>
        /// <exception cref="IzumiNullableMessage.LocalizationByKeyword"></exception>
        private async Task<LocalizationModel> GetLocalizationByKeyword(string keyword)
        {
            // ищем локализацию по ключевому названию
            var localization = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationModel>(@"
                    select * from localizations
                    where name = @keyword",
                    new {keyword});

            // если локализации нет - выводим ошибку
            if (localization == null)
                await Task.FromException(new Exception(IzumiNullableMessage.LocalizationByKeyword.Parse()));

            // возвращаем локализацию
            return localization;
        }

        private async Task<LocalizationModel> GetLocalizationByItemId(LocalizationCategory category, long itemId)
        {
            // ищем локализацию по ключевому названию
            var localization = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<LocalizationModel>(@"
                    select * from localizations
                    where category = @category
                      and item_id = @itemId",
                    new {category, itemId});

            // если локализации нет - выводим ошибку
            if (localization == null)
                await Task.FromException(new Exception(IzumiNullableMessage.LocalizationByKeyword.Parse()));

            // возвращаем локализацию
            return localization;
        }

        private static string Localize(LocalizationModel localization, long amount)
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
