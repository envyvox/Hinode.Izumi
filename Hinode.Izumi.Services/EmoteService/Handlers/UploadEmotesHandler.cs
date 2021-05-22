using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordClientService;
using Hinode.Izumi.Services.EmoteService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.EmoteService.Handlers
{
    public class UploadEmotesHandler : IRequestHandler<UploadEmotesCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;

        public UploadEmotesHandler(IConnectionManager con, IMediator mediator,
            IDiscordClientService discordClientService)
        {
            _con = con;
            _mediator = mediator;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(UploadEmotesCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteOlderEmotesCommand(DateTimeOffset.Now), cancellationToken);

            var socketClient = await _discordClientService.GetSocketClient();
            var emoteId = new List<long>();
            var emoteName = new List<string>();
            var emoteCode = new List<string>();

            foreach (var socketGuild in socketClient.Guilds)
            {
                foreach (var socketGuildEmote in socketGuild.Emotes)
                {
                    emoteId.Add((long) socketGuildEmote.Id);
                    emoteName.Add(socketGuildEmote.Name);
                    emoteCode.Add(socketGuildEmote.ToString());
                }
            }

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into emotes(id, name, code)
                    values (
                            unnest(array[@emoteId]),
                            unnest(array[@emoteName]),
                            unnest(array[@emoteCode])
                            )
                    on conflict (id, name) do update
                        set updated_at = now()",
                    new {emoteId, emoteName, emoteCode});

            return new Unit();
        }
    }
}
