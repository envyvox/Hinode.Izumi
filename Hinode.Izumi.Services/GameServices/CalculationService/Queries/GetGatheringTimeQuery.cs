using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetGatheringTimeQuery(long UserGatheringMastery) : IRequest<long>;
}
