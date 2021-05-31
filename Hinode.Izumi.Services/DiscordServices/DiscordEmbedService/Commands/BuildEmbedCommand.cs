using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Services.GameServices.PremiumService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record BuildEmbedCommand(EmbedBuilder EmbedBuilder, long UserId = 0) : IRequest<Embed>;

    public class BuildEmbedHandler : IRequestHandler<BuildEmbedCommand, Embed>
    {
        private readonly IMediator _mediator;

        public BuildEmbedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Embed> Handle(BuildEmbedCommand request, CancellationToken ct)
        {
            var (embedBuilder, userId) = request;
            var userCommandColor = await _mediator.Send(new GetUserCommandColorQuery(userId), ct);

            return await Task.FromResult(embedBuilder
                .WithColor(new Color(uint.Parse(userCommandColor ?? "36393F", NumberStyles.HexNumber)))
                .Build());
        }
    }
}
