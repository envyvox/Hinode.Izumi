using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.EffectService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetUserEffectsQuery(long UserId) : IRequest<UserEffectRecord[]>;

    public class GetUserEffectsHandler : IRequestHandler<GetUserEffectsQuery, UserEffectRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserEffectsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserEffectRecord[]> Handle(GetUserEffectsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserEffectRecord>(@"
                        select * from user_effects
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
