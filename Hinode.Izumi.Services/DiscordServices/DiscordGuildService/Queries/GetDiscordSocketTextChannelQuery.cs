using Discord.WebSocket;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordSocketTextChannelQuery(long Id) : IRequest<SocketTextChannel>;
}
