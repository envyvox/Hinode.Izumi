using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetCommunityDescChannelsQuery : IRequest<IEnumerable<ulong>>;

    public class GetCommunityDescChannelsHandler : IRequestHandler<GetCommunityDescChannelsQuery, IEnumerable<ulong>>
    {
        private readonly IMediator _mediator;

        public GetCommunityDescChannelsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ulong>> Handle(GetCommunityDescChannelsQuery request, CancellationToken ct)
        {
            var channels = await _mediator.Send(new GetDiscordChannelsQuery(), ct);
            var communityDescChanel = Enum
                .GetValues(typeof(DiscordChannel))
                .Cast<DiscordChannel>()
                .Where(x => x.Parent() == DiscordChannel.CommunityDescParent);

            return channels
                .Where(x => communityDescChanel.Contains(x.Key))
                .Select(x => (ulong) x.Value.Id);
        }
    }
}
