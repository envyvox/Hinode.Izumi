using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetActionTimeQuery(long DefaultTime, int UserEnergy) : IRequest<long>;
}
