using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyOwnerQuery(long FamilyId) : IRequest<UserRecord>;

    public class GetFamilyOwnerHandler : IRequestHandler<GetFamilyOwnerQuery, UserRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyOwnerHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserRecord> Handle(GetFamilyOwnerQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserRecord>(@"
                    select * from users
                    where id = (
                        select user_id from user_families
                        where family_id = @familyId
                          and status = @status)",
                    new {familyId = request.FamilyId, status = UserInFamilyStatus.Head});
        }
    }
}
