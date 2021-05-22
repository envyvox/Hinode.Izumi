using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CooldownService.Queries;
using Hinode.Izumi.Services.GameServices.CooldownService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CooldownService.Handlers
{
    public class GetUserCooldownHandler : IRequestHandler<GetUserCooldownQuery, UserCooldownRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserCooldownHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCooldownRecord> Handle(GetUserCooldownQuery request, CancellationToken cancellationToken)
        {
            var (userId, cooldown) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCooldownRecord>(@"
                    select * from user_cooldowns
                    where user_id = @userId
                      and cooldown = @cooldown",
                    new {userId, cooldown});
        }
    }
}
