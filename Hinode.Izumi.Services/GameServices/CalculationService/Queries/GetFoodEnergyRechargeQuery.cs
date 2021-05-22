using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetFoodEnergyRechargeQuery(long CostPrice, long CookingPrice) : IRequest<long>;
}
