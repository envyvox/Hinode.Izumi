using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record ModifyEmbedCommand(IUserMessage Message, EmbedBuilder EmbedBuilder) : IRequest;

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

            try
            {
                await message.ModifyAsync(x => x.Embed = newEmbed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new Unit();
        }
    }
}
