using MediatR;

namespace Hinode.Izumi.Services.GameServices.AlcoholService.Queries
{
    public record GetAlcoholCostPriceQuery(long Id) : IRequest<long>;
}
