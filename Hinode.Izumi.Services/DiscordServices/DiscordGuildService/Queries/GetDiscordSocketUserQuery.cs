using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries
{
    public record GetDiscordSocketUserQuery(long Id) : IRequest<SocketUser>;

    public class GetDiscordSocketUserHandler : IRequestHandler<GetDiscordSocketUserQuery, SocketUser>
    {
        private readonly IDiscordClientService _discordClientService;

        public GetDiscordSocketUserHandler(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async Task<SocketUser> Handle(GetDiscordSocketUserQuery request, CancellationToken cancellationToken)
        {
            var client = await _discordClientService.GetSocketClient();

            try
            {
                return client.GetUser((ulong) request.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
