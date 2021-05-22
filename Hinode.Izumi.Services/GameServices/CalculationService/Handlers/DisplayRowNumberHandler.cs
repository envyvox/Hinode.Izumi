using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Handlers
{
    public class DisplayRowNumberHandler : IRequestHandler<DisplayRowNumberQuery, string>
    {
        private readonly IMediator _mediator;

        public DisplayRowNumberHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(DisplayRowNumberQuery request, CancellationToken cancellationToken)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            return request.RowNumber switch
            {
                1 => emotes.GetEmoteOrBlank("CupGold"),
                2 => emotes.GetEmoteOrBlank("CupSilver"),
                3 => emotes.GetEmoteOrBlank("CupBronze"),
                < 10 => "🔸",
                _ => "🔹"
            };
        }
    }
}
