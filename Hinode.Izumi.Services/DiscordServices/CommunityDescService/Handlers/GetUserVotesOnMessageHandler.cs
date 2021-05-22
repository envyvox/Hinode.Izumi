using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class GetUserVotesOnMessageHandler
        : IRequestHandler<GetUserVotesOnMessageQuery, Dictionary<Vote, ContentVoteRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserVotesOnMessageHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Vote, ContentVoteRecord>> Handle(GetUserVotesOnMessageQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, messageId) = request;
            return (await _con.GetConnection()
                    .QueryAsync<ContentVoteRecord>(@"
                        select * from content_votes
                        where user_id = @userId
                          and message_id = @messageId",
                        new {userId, messageId}))
                .ToDictionary(x => x.Vote);
        }
    }
}
