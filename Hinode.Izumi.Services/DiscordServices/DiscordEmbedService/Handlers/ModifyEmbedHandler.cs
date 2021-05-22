using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Handlers
{
    public class ModifyEmbedHandler : IRequestHandler<ModifyEmbedCommand>
    {
        private readonly IMediator _mediator;

        public ModifyEmbedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ModifyEmbedCommand request, CancellationToken cancellationToken)
        {
            var (message, embedBuilder) = request;
            var newEmbed = await _mediator.Send(new BuildEmbedCommand(embedBuilder), cancellationToken);

            await message.ModifyAsync(x => x.Embed = newEmbed);

            return new Unit();
        }
    }
}
