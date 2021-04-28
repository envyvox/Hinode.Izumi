using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.RpgServices.CalculationService
{
    public interface ICalculationService
    {
        Task<string> DisplayProgressBar(int number);
        Task<long> TradingSeedDiscount(long userTradingMastery);
        Task<long> TradingTransitDiscount(long userTradingMastery);
        Task<long> TradingSpecialDiscount(long userTradingMastery);
        Task<long> TradingMarketTaxPercent(long userTradingMastery);
        Task<long> SeedPriceWithDiscount(long userTradingMastery, long seedCost);
        Task<long> TransitCostWithDiscount(long userTradingMastery, long transitPrice);
        long ActionTime(long time, int energy);
        Task<long> GatheringTime(long userGatheringMastery);
        Task<long> FishingTime(int energy, bool hasFishingBoat);
        long SuccessAmount(long chance, long doubleChance, long amount);
        Task<double> MasteryXp(MasteryXpProperty property, long userMasteryAmount, long itemsCount = 1);
        Task<double> MasteryFishingXp(long userFishingMastery, bool success);
        Task<FishRarity> FishRarity(long userFishingMastery);
        Task<bool> FishingCheckSuccess(long userFishingMastery, FishRarity rarity);
        Task<long> CraftingPrice(long costPrice);
        Task<long> FoodRecipePrice(long costPrice);
        Task<long> Profit(long npcPrice, long costPrice, long craftingPrice);
        Task<long> NpcPrice(MarketCategory category, long costPrice);
        Task<long> CurrencyAmountAfterMarketTax(long userTradingMastery, long amount);
        Task<long> CraftingAmountAfterMasteryProcs(long craftingId, long userCraftingMastery, long amount);
        Task<long> AlcoholAmountAfterMasteryProcs(long alcoholId, long userCraftingMastery, long amount);
        Task<long> DrinkAmountAfterMasteryProcs(long drinkId, long userCraftingMastery, long amount);
    }
}
