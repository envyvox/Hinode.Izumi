using MediatR;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Queries
{
    public record GetCraftingCostPriceQuery(long Id) : IRequest<long>;
}
