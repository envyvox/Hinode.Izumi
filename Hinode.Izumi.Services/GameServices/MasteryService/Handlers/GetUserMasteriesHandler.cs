using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.MasteryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Handlers
{
    public class GetUserMasteriesHandler
        : IRequestHandler<GetUserMasteriesQuery, Dictionary<Mastery, UserMasteryRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserMasteriesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Mastery, UserMasteryRecord>> Handle(GetUserMasteriesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserMasteryRecord>(@"
                        select * from user_masteries
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Mastery);
        }
    }
}
