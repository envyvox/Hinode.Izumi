using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReputationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record GetUserReputationsQuery(long UserId) : IRequest<Dictionary<Reputation, UserReputationRecord>>;

    public class GetUserReputationsHandler
        : IRequestHandler<GetUserReputationsQuery, Dictionary<Reputation, UserReputationRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserReputationsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Reputation, UserReputationRecord>> Handle(GetUserReputationsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserReputationRecord>(@"
                        select * from user_reputations
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Reputation);
        }
    }
}
