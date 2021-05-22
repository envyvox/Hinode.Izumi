using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService.Options;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Handlers
{
    public class GetDiscordSocketGuildHandler : IRequestHandler<GetDiscordSocketGuildQuery, SocketGuild>
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly IOptions<DiscordOptions> _options;

        public GetDiscordSocketGuildHandler(IDiscordClientService discordClientService, IOptions<DiscordOptions> options)
        {
            _discordClientService = discordClientService;
            _options = options;
        }

        public async Task<SocketGuild> Handle(GetDiscordSocketGuildQuery request, CancellationToken cancellationToken)
        {
            var client = await _discordClientService.GetSocketClient();
            return client.GetGuild(_options.Value.GuildId);
        }
    }
}
