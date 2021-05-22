using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Handlers
{
    public class BuildEmbedHandler : IRequestHandler<BuildEmbedCommand, Embed>
    {
        public async Task<Embed> Handle(BuildEmbedCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.EmbedBuilder
                .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                .Build());
        }
    }
}
