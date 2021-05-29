using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Queries
{
    public record CheckUserHasReferrerQuery(long UserId) : IRequest<bool>;

    public class CheckUserHasReferrerHandler : IRequestHandler<CheckUserHasReferrerQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasReferrerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasReferrerQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_referrers
                    where user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
