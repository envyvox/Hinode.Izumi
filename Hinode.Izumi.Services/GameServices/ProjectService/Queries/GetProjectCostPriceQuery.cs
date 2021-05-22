using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Queries
{
    public record GetProjectCostPriceQuery(long Id) : IRequest<long>;
}
