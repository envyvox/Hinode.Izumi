using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentMessageVotesQuery(long ContentMessageId, Vote Vote) : IRequest<ContentVoteRecord[]>;

    public class GetContentMessageVotesHandler : IRequestHandler<GetContentMessageVotesQuery, ContentVoteRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetContentMessageVotesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContentVoteRecord[]> Handle(GetContentMessageVotesQuery request,
            CancellationToken cancellationToken)
        {
            var (messageId, vote) = request;
            return (await _con.GetConnection()
                    .QueryAsync<ContentVoteRecord>(@"
                        select * from content_votes
                        where message_id = @messageId
                          and vote = @vote
                          and active = true",
                        new {messageId, vote}))
                .ToArray();
        }
    }
}
