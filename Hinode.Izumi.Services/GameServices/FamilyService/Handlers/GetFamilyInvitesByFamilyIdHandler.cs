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
    public class GetFamilyInvitesByFamilyIdHandler
        : IRequestHandler<GetFamilyInvitesByFamilyIdQuery, FamilyInviteRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetFamilyInvitesByFamilyIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyInviteRecord[]> Handle(GetFamilyInvitesByFamilyIdQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<FamilyInviteRecord>(@"
                        select * from family_invites
                        where family_id = @familyId",
                        new {familyId = request.FamilyId}))
                .ToArray();
        }
    }
}
