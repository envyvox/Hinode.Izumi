using Hinode.Izumi.Data.Enums.RarityEnums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record CheckFishingSuccessQuery(long UserFishingMastery, FishRarity Rarity) : IRequest<bool>;
}
