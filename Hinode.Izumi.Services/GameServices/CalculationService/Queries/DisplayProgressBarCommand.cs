using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CalculationService.Queries
{
    public record DisplayProgressBarCommand(int Number) : IRequest<string>;

    public class DisplayProgressBarHandler : IRequestHandler<DisplayProgressBarCommand, string>
    {
        private readonly IMediator _mediator;

        public DisplayProgressBarHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Handle(DisplayProgressBarCommand request, CancellationToken cancellationToken)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var bars = new Dictionary<long, string>
            {
                {
                    0,
                    $"{emotes.GetEmoteOrBlank("RedCutStart")}{emotes.GetEmoteOrBlank("RedCutEnd")}"
                },
                {
                    10,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    20,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    30,
                    $"{emotes.GetEmoteOrBlank("RedStart")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedFull")}{emotes.GetEmoteOrBlank("RedEnd")}"
                },
                {
                    40,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    50,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    60,
                    $"{emotes.GetEmoteOrBlank("YellowStart")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowFull")}{emotes.GetEmoteOrBlank("YellowEnd")}"
                },
                {
                    70,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    80,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    90,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                },
                {
                    100,
                    $"{emotes.GetEmoteOrBlank("GreenStart")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenFull")}{emotes.GetEmoteOrBlank("GreenEnd")}"
                }
            };

            return bars[bars.Keys.Where(x => x <= request.Number).Max()];
        }
    }
}
