using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.StatisticService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.StatisticService.Handlers
{
    public class GetUserStatisticHandler : IRequestHandler<GetUserStatisticQuery, UserStatisticRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserStatisticHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserStatisticRecord> Handle(GetUserStatisticQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, statistic) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserStatisticRecord>(@"
                    select * from user_statistics
                    where user_id = @userId
                      and statistic = @statistic",
                    new {userId, statistic});
        }
    }
}
