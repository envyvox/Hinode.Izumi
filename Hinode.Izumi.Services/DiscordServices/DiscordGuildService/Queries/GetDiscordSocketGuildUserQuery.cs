using Discord.WebSocket;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordSocketGuildUserQuery(long Id) : IRequest<SocketGuildUser>;
}
