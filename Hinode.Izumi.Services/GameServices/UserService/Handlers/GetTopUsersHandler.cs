using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
    public class GetTopUsersHandler : IRequestHandler<GetTopUsersQuery, UserWithRowNumberRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetTopUsersHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserWithRowNumberRecord[]> Handle(GetTopUsersQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserWithRowNumberRecord>(@"
                        select * from (
                            select *,
                                   row_number() over (order by points desc, created_at desc) as RowNumber
                            from users) tmp
                        limit 10"))
                .ToArray();
        }
    }
}
