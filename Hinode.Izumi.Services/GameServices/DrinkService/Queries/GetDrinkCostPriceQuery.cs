using MediatR;

namespace Hinode.Izumi.Services.GameServices.DrinkService.Queries
{
    public record GetDrinkCostPriceQuery(long Id) : IRequest<long>;
}
