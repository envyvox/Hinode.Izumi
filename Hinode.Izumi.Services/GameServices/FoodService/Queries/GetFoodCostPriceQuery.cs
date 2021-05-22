using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetFoodCostPriceQuery(long Id) : IRequest<long>;
}
