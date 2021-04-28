using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.PropertyService;

namespace Hinode.Izumi.Services.RpgServices.CalculationService.Impl
{
    [InjectableService]
    public class CalculationService : ICalculationService
    {
        private readonly IEmoteService _emoteService;
        private readonly IPropertyService _propertyService;
        private readonly IIngredientService _ingredientService;

        private static readonly Random Random = new();

        public CalculationService(IEmoteService emoteService, IPropertyService propertyService,
            IIngredientService ingredientService)
        {
            _emoteService = emoteService;
            _propertyService = propertyService;
            _ingredientService = ingredientService;
        }

        public async Task<string> DisplayProgressBar(int number)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // заполняем библиотеку прогресс-барами
            var bars = new Dictionary<long, string>
            {
                {
                    0,
                    $"{emotes.GetEmoteOrBlank("RedCutStart")}{emotes.GetEmoteOrBlank("RedCutEnd")}"
                },
                {
                    10,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    20,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    30,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    40,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    50,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    60,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    70,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    80,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    90,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    100,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                }
            };
            // получаем необходимый прогресс-бар
            return bars[bars.Keys.Where(x => x <= number).Max()];
        }

        public async Task<long> TradingSeedDiscount(long userTradingMastery) =>
            (await _propertyService.GetMasteryProperty(MasteryProperty.TradingMasterySeedDiscount))
            .MasteryMaxValue(userTradingMastery);

        public async Task<long> TradingTransitDiscount(long userTradingMastery) =>
            (await _propertyService.GetMasteryProperty(MasteryProperty.TradingMasteryTransitDiscount))
            .MasteryMaxValue(userTradingMastery);

        public async Task<long> TradingSpecialDiscount(long userTradingMastery) =>
            (await _propertyService.GetMasteryProperty(MasteryProperty.TradingMasterySpecialOfferDiscount))
            .MasteryMaxValue(userTradingMastery);

        public async Task<long> TradingMarketTaxPercent(long userTradingMastery) =>
            (await _propertyService.GetMasteryProperty(MasteryProperty.TradingMasteryMarketTax))
            .MasteryMaxValue(userTradingMastery);

        public async Task<long> SeedPriceWithDiscount(long userTradingMastery, long seedCost)
        {
            // если мастерство пользователя меньше 50 - возвращаем стоимость по-умолчанию
            if (userTradingMastery < 50) return seedCost;
            // получаем % скидки
            var discount =
                (await _propertyService.GetMasteryProperty(MasteryProperty.TradingMasterySeedDiscount))
                .MasteryMaxValue(userTradingMastery);
            // отнимаем от стоимости по-умолчанию % скидки
            return seedCost - (
                // убеждаемся что финальная сумма не меньше 0
                seedCost * discount / 100 > 0
                    ? seedCost * discount / 100
                    : 1
            );
        }

        public async Task<long> TransitCostWithDiscount(long userTradingMastery, long transitPrice)
        {
            if (transitPrice == 0) return 0;
            var discount = await TradingTransitDiscount(userTradingMastery);
            return transitPrice - (transitPrice * discount / 100 > 0
                ? transitPrice * discount / 100
                : 1);
        }

        public long ActionTime(long time, int energy) =>
            Framework.Extensions.DictionaryExtensions.MaxValue(new Dictionary<long, long>
            {
                {0, time + time * 50 / 100},
                {10, time + time * 25 / 100},
                {40, time},
                {70, time - time * 25 / 100}
            }, energy);

        public async Task<long> GatheringTime(long userGatheringMastery) =>
            (await _propertyService.GetMasteryProperty(MasteryProperty.ActionTimeGathering))
            .MasteryMaxValue(userGatheringMastery);

        public async Task<long> FishingTime(int energy, bool hasFishingBoat)
        {
            // получаем стандартное время рыбалки
            var time = await _propertyService.GetPropertyValue(Property.ActionTimeFishing);
            // если у пользователя есть рыбацкая лодка, то нужно уменьшить это значение
            if (hasFishingBoat) time -= await _propertyService.GetPropertyValue(Property.ActionTimeReduceFishingBoat);
            // возвращаем время рыбалки с учетом энергии пользователя
            return ActionTime(time, energy);
        }

        public long SuccessAmount(long chance, long doubleChance, long amount) =>
            chance >= Random.Next(1, 101)
                ? doubleChance >= Random.Next(1, 101)
                    ? amount * 2
                    : amount
                : 0;

        public async Task<double> MasteryXp(MasteryXpProperty property, long userMasteryAmount, long itemsCount = 1) =>
            (await _propertyService.GetMasteryXpProperty(property))
            .MasteryXpMaxValue(userMasteryAmount) * itemsCount;

        public async Task<double> MasteryFishingXp(long userFishingMastery, bool success) =>
            (await _propertyService.GetMasteryXpProperty(success
                ? MasteryXpProperty.FishingSuccess
                : MasteryXpProperty.FishingFail))
            .MasteryXpMaxValue(userFishingMastery);

        public async Task<FishRarity> FishRarity(long userFishingMastery)
        {
            // получаем редкости рыбы
            var rarities = Enum.GetValues(typeof(FishRarity))
                .Cast<FishRarity>()
                .ToArray();

            // копипаста, не уверен точно как она работает ^^"
            while (true)
            {
                var rand = Random.Next(1, 101);
                long current = 0;
                foreach (var rarity in rarities)
                {
                    var chance = rarity switch
                    {
                        Data.Enums.RarityEnums.FishRarity.Common =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceCommon))
                            .MasteryMaxValue(userFishingMastery),

                        Data.Enums.RarityEnums.FishRarity.Rare =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceRare))
                            .MasteryMaxValue(userFishingMastery),

                        Data.Enums.RarityEnums.FishRarity.Epic =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceEpic))
                            .MasteryMaxValue(userFishingMastery),

                        Data.Enums.RarityEnums.FishRarity.Mythical =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceMythical))
                            .MasteryMaxValue(userFishingMastery),

                        Data.Enums.RarityEnums.FishRarity.Legendary =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceLegendary))
                            .MasteryMaxValue(userFishingMastery),

                        Data.Enums.RarityEnums.FishRarity.Divine =>
                            (await _propertyService.GetMasteryProperty(MasteryProperty.FishRarityChanceDivine))
                            .MasteryMaxValue(userFishingMastery),

                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (current <= rand && rand < current + chance) return rarity;
                    current += chance;
                }
            }
        }

        public async Task<bool> FishingCheckSuccess(long userFishingMastery, FishRarity rarity)
        {
            // получаем шанс срыва в зависимости от редкости рыбы
            var chance = rarity switch
            {
                Data.Enums.RarityEnums.FishRarity.Common =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceCommon))
                    .MasteryMaxValue(userFishingMastery),

                Data.Enums.RarityEnums.FishRarity.Rare =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceRare))
                    .MasteryMaxValue(userFishingMastery),

                Data.Enums.RarityEnums.FishRarity.Epic =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceEpic))
                    .MasteryMaxValue(userFishingMastery),

                Data.Enums.RarityEnums.FishRarity.Mythical =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceMythical))
                    .MasteryMaxValue(userFishingMastery),

                Data.Enums.RarityEnums.FishRarity.Legendary =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceLegendary))
                    .MasteryMaxValue(userFishingMastery),

                Data.Enums.RarityEnums.FishRarity.Divine =>
                    (await _propertyService.GetMasteryProperty(MasteryProperty.FishFailChanceDivine))
                    .MasteryMaxValue(userFishingMastery),

                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
            // проверяем на успех и возвращаем
            return Random.Next(1, 101) > chance;
        }

        public async Task<long> CraftingPrice(long costPrice)
        {
            // получаем % стоимости изготовления
            var craftingCost = await _propertyService.GetPropertyValue(Property.CraftingCost);
            return (long)
                // стоимость изготовления не может быть меньше 1
                (costPrice / 100.0 * craftingCost < 1
                    ? 1
                    // стоимость изготовления это % от себестоимости
                    : costPrice / 100.0 * craftingCost
                );
        }

        public async Task<long> FoodRecipePrice(long costPrice)
        {
            // получаем стоимость приготовления блюда
            var craftingPrice = await CraftingPrice(costPrice);
            // получаем цену NPC
            var npcPrice = await NpcPrice(MarketCategory.Food, costPrice);
            // получаем чистый профит
            var profit = await Profit(npcPrice, costPrice, craftingPrice);
            // цена рецепта это чистый профит * количество продаж блюда для окупаемости рецепта
            return profit * await _propertyService.GetPropertyValue(Property.RecipePaybackSales);
        }

        public async Task<long> Profit(long npcPrice, long costPrice, long craftingPrice) =>
            // получаем стоимость после вычета налога рынка (мастерство в данном контекте не учитывается, поэтому 0)
            (long) (npcPrice - npcPrice / 100.0 * await TradingMarketTaxPercent(0) -
                    // и отнимаем себестоимость с учетом стоимости изготовления
                    (costPrice + craftingPrice));

        public async Task<long> NpcPrice(MarketCategory category, long costPrice)
        {
            // получаем стоимость изготовления
            var craftingPrice = await CraftingPrice(costPrice);
            return (long)
                // цена NPC это себестоимость + стоимость изготовления умноженная на особый %
                (costPrice + craftingPrice + (costPrice + craftingPrice) / 100.0 *
                    // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
                    category switch
                    {
                        // определяем особый % в зависимости от категории
                        MarketCategory.Crafting => await _propertyService.GetPropertyValue(Property.CraftingMarkup),
                        MarketCategory.Alcohol => await _propertyService.GetPropertyValue(Property.AlcoholMarkup),
                        MarketCategory.Drink => await _propertyService.GetPropertyValue(Property.DrinkMarkup),
                        MarketCategory.Food => await _propertyService.GetPropertyValue(Property.FoodMarkup),
                        _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
                    });
        }

        public async Task<long> CurrencyAmountAfterMarketTax(long userTradingMastery, long amount) =>
            amount - amount * await TradingMarketTaxPercent(userTradingMastery) / 100;

        public async Task<long> CraftingAmountAfterMasteryProcs(long craftingId, long userCraftingMastery, long amount)
        {
            // получаем шанс на двойное изготовление
            var chance =
                (await _propertyService.GetCraftingProperty(craftingId, CraftingProperty.CraftingDoubleChance))
                .MasteryMaxValue(userCraftingMastery);
            // устанавливаем возвращаемое значение - количество изготовлений
            var returnAmount = amount;

            // для каждого изготовления
            for (var i = 0; i < amount; i++)
                // проверяем шанс двойного изготовления
                if (chance >= Random.Next(0, 101))
                    // и добавляем бонусную единицу если шанс прошел
                    returnAmount++;

            // возвращаем итоговое значение
            return returnAmount;
        }

        public async Task<long> AlcoholAmountAfterMasteryProcs(long alcoholId, long userCraftingMastery, long amount)
        {
            // получаем шанс на двойное изготовление
            var chance =
                (await _propertyService.GetAlcoholProperty(alcoholId, AlcoholProperty.CraftingDoubleChance))
                .MasteryMaxValue(userCraftingMastery);
            // устанавливаем возвращаемое значение - количество изготовлений
            var returnAmount = amount;

            // для каждого изготовления
            for (var i = 0; i < amount; i++)
                // проверяем шанс двойного изготовления
                if (chance >= Random.Next(0, 101))
                    // и добавляем бонусную единицу если шанс прошел
                    returnAmount++;

            // возвращаем итоговое значение
            return returnAmount;
        }

        public async Task<long> DrinkAmountAfterMasteryProcs(long drinkId, long userCraftingMastery, long amount)
        {
            // TODO добавить сюда проверки после того как обсудим работу изготовления напитков
            return await Task.FromResult(amount);
        }
    }
}
