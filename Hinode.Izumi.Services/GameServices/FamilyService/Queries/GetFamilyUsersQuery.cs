using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyUsersQuery(long FamilyId) : IRequest<UserFamilyRecord[]>;

    public class GetFamilyUsersHandler : IRequestHandler<GetFamilyUsersQuery, UserFamilyRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetFamilyUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserFamilyRecord[]> Handle(GetFamilyUsersQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserFamilyRecord>(@"
                        select * from user_families
                        where family_id = @familyId
                        order by status desc",
                        new {familyId = request.FamilyId}))
                .ToArray();
        }
    }
}
