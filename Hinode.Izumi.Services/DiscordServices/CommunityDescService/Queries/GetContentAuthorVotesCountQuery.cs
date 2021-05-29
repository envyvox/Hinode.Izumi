using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries
{
    public record GetContentAuthorVotesCountQuery(long UserId, Vote Vote) : IRequest<long>;

    public class GetContentAuthorVotesCountHandler : IRequestHandler<GetContentAuthorVotesCountQuery, long>
    {
        private readonly IConnectionManager _con;

        public GetContentAuthorVotesCountHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<long> Handle(GetContentAuthorVotesCountQuery request, CancellationToken cancellationToken)
        {
            var (userId, vote) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from content_votes
                    where message_id in (
                        select id from content_messages cm where cm.user_id = @userId
                        )
                      and vote = @vote
                      and active = true",
                    new {userId, vote});
        }
    }
}
