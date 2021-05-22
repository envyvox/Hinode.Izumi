using Discord;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands
{
    public record BuildEmbedCommand(EmbedBuilder EmbedBuilder) : IRequest<Embed>;
}
