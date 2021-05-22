using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetMasteryFishingXpQuery(long UserFishingMastery, bool Success) : IRequest<double>;
}
