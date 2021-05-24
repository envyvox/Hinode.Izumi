using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyByIdQuery(long Id) : IRequest<FamilyRecord>;

    public class GetFamilyByIdHandler : IRequestHandler<GetFamilyByIdQuery, FamilyRecord>
    {
        private readonly IConnectionManager _con;

        public GetFamilyByIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<FamilyRecord> Handle(GetFamilyByIdQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyRecord>(@"
                    select * from families
                    where id = @id",
                    new {id = request.Id});
        }
    }
}
