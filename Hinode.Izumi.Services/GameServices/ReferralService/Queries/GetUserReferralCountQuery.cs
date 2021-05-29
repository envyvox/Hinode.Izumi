using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record GetUserReferralCountQuery(long UserId) : IRequest<long>;

    public class GetUserReferralCountHandler : IRequestHandler<GetUserReferralCountQuery, long>
    {
        private readonly IConnectionManager _con;

        public GetUserReferralCountHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<long> Handle(GetUserReferralCountQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from user_referrers
                    where referrer_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
