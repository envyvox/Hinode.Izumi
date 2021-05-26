using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Records;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentAuthorVotesQuery(long UserId) : IRequest<ContentVoteRecord[]>;

    public class GetContentAuthorVotesHandler : IRequestHandler<GetContentAuthorVotesQuery, ContentVoteRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetContentAuthorVotesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ContentVoteRecord[]> Handle(GetContentAuthorVotesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ContentVoteRecord>(@"
                        select * from content_votes
                        where message_id in (
                            select id from content_messages cm where cm.user_id = @userId
                            )
                          and active = true",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
