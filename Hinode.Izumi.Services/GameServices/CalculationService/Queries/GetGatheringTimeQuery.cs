using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetGatheringTimeQuery(long UserGatheringMastery) : IRequest<long>;

    public class GetGatheringTimeHandler : IRequestHandler<GetGatheringTimeQuery, long>
    {
        private readonly IMediator _mediator;

        public GetGatheringTimeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetGatheringTimeQuery request, CancellationToken cancellationToken)
        {
            return (await _mediator.Send(
                    new GetMasteryPropertiesQuery(MasteryProperty.ActionTimeGathering), cancellationToken))
                .MasteryMaxValue(request.UserGatheringMastery);
        }
    }
}
