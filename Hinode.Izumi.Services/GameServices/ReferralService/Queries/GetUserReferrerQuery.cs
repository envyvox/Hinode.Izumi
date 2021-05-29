using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record GetUserReferrerQuery(long UserId) : IRequest<UserRecord>;

    public class GetUserReferrerHandler : IRequestHandler<GetUserReferrerQuery, UserRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserReferrerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord> Handle(GetUserReferrerQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    select u.* from user_referrers as ur
                        inner join users u
                            on u.id = ur.referrer_id
                    where ur.user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
