using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class GetFamilyInvitesByUserIdHandler : IRequestHandler<GetFamilyInvitesByUserIdQuery, FamilyInviteRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetFamilyInvitesByUserIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyInviteRecord[]> Handle(GetFamilyInvitesByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FamilyInviteRecord>(@"
                        select * from family_invites
                        where user_id = @userId",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
