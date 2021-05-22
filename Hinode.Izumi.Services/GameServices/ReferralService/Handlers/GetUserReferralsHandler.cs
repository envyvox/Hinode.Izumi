using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReferralService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Handlers
{
    public class GetUserReferralsHandler : IRequestHandler<GetUserReferralsQuery, UserRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserReferralsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord[]> Handle(GetUserReferralsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserRecord>(@"
                        select u.* from user_referrers as ur
                            inner join users u
                                on u.id = ur.user_id
                        where ur.referrer_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
