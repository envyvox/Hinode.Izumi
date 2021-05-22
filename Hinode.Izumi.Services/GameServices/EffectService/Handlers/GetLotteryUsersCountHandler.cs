using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Handlers
{
    public class GetLotteryUsersCountHandler : IRequestHandler<GetLotteryUsersCountQuery, long>
    {
        private readonly IConnectionManager _con;

        public GetLotteryUsersCountHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<long> Handle(GetLotteryUsersCountQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select count(*) from user_effects
                    where effect = @lotteryEffect",
                    new {lotteryEffect = Effect.Lottery});
        }
    }
}
