using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record CheckUserHasFamilyQuery(long UserId) : IRequest<bool>;

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
