using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class GetCommunityDescChannelsHandler : IRequestHandler<GetCommunityDescChannelsQuery, List<ulong>>
    {
        private readonly IMediator _mediator;

        public GetCommunityDescChannelsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<ulong>> Handle(GetCommunityDescChannelsQuery request,
            CancellationToken cancellationToken)
        {
            var channels = await _mediator.Send(new GetDiscordChannelsQuery(), cancellationToken);
            return new List<ulong>
            {
                (ulong) channels[DiscordChannel.Screenshots].Id,
                (ulong) channels[DiscordChannel.Memes].Id,
                (ulong) channels[DiscordChannel.Arts].Id,
                (ulong) channels[DiscordChannel.Erotic].Id,
                (ulong) channels[DiscordChannel.Nsfw].Id
            };
        }
    }
}
