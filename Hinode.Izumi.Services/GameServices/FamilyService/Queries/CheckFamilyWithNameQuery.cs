using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record CheckFamilyWithNameQuery(string Name) : IRequest<bool>;

    public class CheckFamilyWithNameHandler : IRequestHandler<CheckFamilyWithNameQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckFamilyWithNameHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckFamilyWithNameQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from families
                    where name = @name",
                    new {name = request.Name});
        }
    }
}
