using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetFoodRecipePriceQuery(long CostPrice) : IRequest<long>;
}
