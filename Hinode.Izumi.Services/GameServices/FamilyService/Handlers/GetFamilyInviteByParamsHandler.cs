using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class GetFamilyInviteByParamsHandler : IRequestHandler<GetFamilyInviteByParamsQuery, FamilyInviteRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyInviteByParamsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyInviteRecord> Handle(GetFamilyInviteByParamsQuery request,
            CancellationToken cancellationToken)
        {
            var (familyId, userId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyInviteRecord>(@"
                    select * from family_invites
                    where family_id = @familyId
                      and user_id = @userId",
                    new {familyId, userId});
        }
    }
}
