using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class GetMasteryXpHandler : IRequestHandler<GetMasteryXpQuery, double>
    {
        private readonly IMediator _mediator;

        public GetMasteryXpHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<double> Handle(GetMasteryXpQuery request, CancellationToken cancellationToken)
        {
            var (property, userMasteryAmount, itemsCount) = request;
            return (await _mediator.Send(new GetMasteryXpPropertiesQuery(property), cancellationToken))
                .MasteryXpMaxValue(userMasteryAmount) * itemsCount;
        }
    }
}
