using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetProfitQuery(long CostPrice, long CraftingPrice, long NpcPrice) : IRequest<long>;
}
