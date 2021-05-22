using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
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
            return client.GetUser((ulong) request.Id);
        }
    }
}
