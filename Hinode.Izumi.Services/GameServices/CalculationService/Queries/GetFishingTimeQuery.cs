using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetFishingTimeQuery(int UserEnergy, bool HasFishingBoat) : IRequest<long>;
}
