using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetCraftingPriceQuery(long CostPrice, long Amount = 1) : IRequest<long>;
}
