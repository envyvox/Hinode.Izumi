using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class CheckUserHasFamilyHandler : IRequestHandler<CheckUserHasFamilyQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasFamilyHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasFamilyQuery request, CancellationToken cancellationToken)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_families
                    where user_id = @userId",
                    new {userId = request.UserId});
        }
    }
}
