using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Handlers
{
    public class GetRandomLotteryWinnerHandler : IRequestHandler<GetRandomLotteryWinnerQuery, UserRecord>
    {
        private readonly IConnectionManager _con;

        public GetRandomLotteryWinnerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord> Handle(GetRandomLotteryWinnerQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    select * from users
                    where id = (
                        select user_id from user_effects
                        where effect = @lotteryEffect
                        order by random()
                        limit 1)",
                    new {lotteryEffect = Effect.Lottery});
        }
    }
}
