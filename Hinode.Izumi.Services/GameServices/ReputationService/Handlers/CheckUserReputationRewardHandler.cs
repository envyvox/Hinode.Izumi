using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Handlers
{
    public class CheckUserReputationRewardHandler : IRequestHandler<CheckUserReputationRewardQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserReputationRewardHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserReputationRewardQuery request, CancellationToken cancellationToken)
        {
            var (userId, reputation, amount) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_reputation_rewards
                    where user_id = @userId
                      and reputation = @reputation
                      and amount = @amount",
                    new {userId, reputation, amount});
        }
    }
}
