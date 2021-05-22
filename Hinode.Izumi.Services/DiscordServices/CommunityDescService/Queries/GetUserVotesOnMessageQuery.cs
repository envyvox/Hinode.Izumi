using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetUserVotesOnMessageQuery(
            long UserId,
            long MessageId)
        : IRequest<Dictionary<Vote, ContentVoteRecord>>;
}
