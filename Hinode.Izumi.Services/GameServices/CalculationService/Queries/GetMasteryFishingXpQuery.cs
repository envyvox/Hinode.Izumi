using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetMasteryFishingXpQuery(long UserFishingMastery, bool Success) : IRequest<double>;

    public class GetMasteryFishingXpHandler : IRequestHandler<GetMasteryFishingXpQuery, double>
    {
        private readonly IMediator _mediator;

        public GetMasteryFishingXpHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<double> Handle(GetMasteryFishingXpQuery request, CancellationToken cancellationToken)
        {
            var (userFishingMastery, success) = request;
            return (await _mediator.Send(new GetMasteryXpPropertiesQuery(success
                    ? MasteryXpProperty.FishingSuccess
                    : MasteryXpProperty.FishingFail), cancellationToken))
                .MasteryXpMaxValue(userFishingMastery);
        }
    }
}
