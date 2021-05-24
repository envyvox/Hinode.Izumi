using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReputationService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Queries
{
    public record GetUserReputationQuery(long UserId, Reputation Reputation) : IRequest<UserReputationRecord>;

    public class GetUserReputationHandler : IRequestHandler<GetUserReputationQuery, UserReputationRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserReputationHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserReputationRecord> Handle(GetUserReputationQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, reputation) = request;
            return await _con.GetConnection()
                       .QueryFirstOrDefaultAsync<UserReputationRecord>(@"
                            select * from user_reputations
                            where user_id = @userId
                              and reputation = @reputation",
                           new {userId, reputation})
                   ?? new UserReputationRecord(userId, reputation, 0);
        }
    }
}
