using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentMessageVotesQuery(long ContentMessageId, Vote Vote) : IRequest<ContentVoteRecord[]>;
}
