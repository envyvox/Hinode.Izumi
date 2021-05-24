using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetFoodEnergyRechargeQuery(long CostPrice, long CookingPrice) : IRequest<long>;

    public class GetFoodEnergyRechargeHandler : IRequestHandler<GetFoodEnergyRechargeQuery, long>
    {
        private readonly IMediator _mediator;

        public GetFoodEnergyRechargeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetFoodEnergyRechargeQuery request, CancellationToken cancellationToken)
        {
            var (costPrice, cookingPrice) = request;
            var foodEnergyPrice = await _mediator.Send(
                new GetPropertyValueQuery(Property.FoodEnergyPrice), cancellationToken);

            return (costPrice + cookingPrice) / foodEnergyPrice + 2;
        }
    }
}
