using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.CommunityDescService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.DiscordServices.CommunityDescService.Handlers
{
    public class GetContentAuthorVotesHandler : IRequestHandler<GetContentAuthorVotesQuery, long>
    {
        private readonly IConnectionManager _con;

        public GetContentAuthorVotesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<long> Handle(GetContentAuthorVotesQuery request, CancellationToken cancellationToken)
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
