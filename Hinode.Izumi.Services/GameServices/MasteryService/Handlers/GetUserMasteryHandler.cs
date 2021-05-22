using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Handlers
{
    public class GetUserMasteryHandler : IRequestHandler<GetUserMasteryQuery, UserMasteryRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserMasteryHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserMasteryRecord> Handle(GetUserMasteryQuery request, CancellationToken cancellationToken)
        {
            var (userId, mastery) = request;
            return await _con.GetConnection()
                       .QueryFirstOrDefaultAsync<UserMasteryRecord>(@"
                        select * from user_masteries
                        where user_id = @userId
                          and mastery = @mastery",
                           new {userId, mastery})
                   ?? new UserMasteryRecord(userId, mastery, 0);
        }
    }
}
