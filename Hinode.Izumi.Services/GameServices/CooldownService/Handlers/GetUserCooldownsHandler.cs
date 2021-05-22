using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CooldownService.Queries;
using Hinode.Izumi.Services.GameServices.CooldownService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Handlers
{
    public class GetUserCooldownsHandler
        : IRequestHandler<GetUserCooldownsQuery, Dictionary<Cooldown, UserCooldownRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserCooldownsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Cooldown, UserCooldownRecord>> Handle(GetUserCooldownsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserCooldownRecord>(@"
                        select * from user_cooldowns
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Cooldown);
        }
    }
}
