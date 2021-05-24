using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Framework.Extensions;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record GetActionTimeQuery(long DefaultTime, int UserEnergy) : IRequest<long>;

    public class GetActionTimeHandler : IRequestHandler<GetActionTimeQuery, long>
    {
        private readonly IMediator _mediator;

        public GetActionTimeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<long> Handle(GetActionTimeQuery request, CancellationToken cancellationToken)
        {
            var (defaultTime, userEnergy) = request;
            return await Task.FromResult(new Dictionary<long, long>
            {
                {0, defaultTime + defaultTime * 50 / 100},
                {10, defaultTime + defaultTime * 25 / 100},
                {40, defaultTime},
                {70, defaultTime - defaultTime * 25 / 100},
                {85, defaultTime - defaultTime * 50 / 100}
            }.MaxValue(userEnergy));
        }
    }
}
