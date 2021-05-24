using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetFishingTimeQuery(int UserEnergy, bool HasFishingBoat) : IRequest<long>;

    public class GetFishingTimeHandler : IRequestHandler<GetFishingTimeQuery, long>
    {
        private readonly IMediator _mediator;

        public GetFishingTimeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetFishingTimeQuery request, CancellationToken cancellationToken)
        {
            var (userEnergy, hasFishingBoat) = request;
            var defaultTime = await _mediator.Send(
                new GetPropertyValueQuery(Property.ActionTimeFishing), cancellationToken);
            var fishingBoatTimeReduce = await _mediator.Send(
                new GetPropertyValueQuery(Property.ActionTimeReduceFishingBoat), cancellationToken);

            if (hasFishingBoat) defaultTime -= fishingBoatTimeReduce;

            return await _mediator.Send(new GetActionTimeQuery(defaultTime, userEnergy), cancellationToken);
        }
    }
}
