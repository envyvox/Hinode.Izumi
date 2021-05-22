using System.Collections.Generic;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetCommunityDescChannelsQuery : IRequest<List<ulong>>;
}
