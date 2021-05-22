using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetNpcPriceQuery(MarketCategory Category, long CostPrice) : IRequest<long>;
}
