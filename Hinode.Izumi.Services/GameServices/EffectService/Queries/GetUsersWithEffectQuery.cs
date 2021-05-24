using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Queries
{
    public record GetUsersWithEffectQuery(Effect Effect) : IRequest<UserRecord[]>;

    public class GetUsersWithEffectHandler : IRequestHandler<GetUsersWithEffectQuery, UserRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUsersWithEffectHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord[]> Handle(GetUsersWithEffectQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserRecord>(@"
                        select u.* from user_effects as ue
                            inner join users u
                                on u.id = ue.user_id
                        where ue.effect = @effect",
                        new {effect = request.Effect}))
                .ToArray();
        }
    }
}
