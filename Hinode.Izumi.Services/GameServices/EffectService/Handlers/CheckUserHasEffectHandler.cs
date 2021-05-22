using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Handlers
{
    public class CheckUserHasEffectHandler : IRequestHandler<CheckUserHasEffectQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasEffectHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasEffectQuery request, CancellationToken cancellationToken)
        {
            var (userId, effect) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_effects
                    where user_id = @userId
                      and effect = @effect",
                    new {userId, effect});
        }
    }
}
