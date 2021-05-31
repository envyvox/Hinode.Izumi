using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.HangfireJobService.Queries
{
    public record GetUserHangfireJobIdQuery(long UserId, HangfireAction Action) : IRequest<string>;

    public class GetUserHangfireJobIdHandler : IRequestHandler<GetUserHangfireJobIdQuery, string>
    {
        private readonly IConnectionManager _con;

        public GetUserHangfireJobIdHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<string> Handle(GetUserHangfireJobIdQuery request, CancellationToken cancellationToken)
        {
            var (userId, action) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select job_id from user_hangfire_jobs
                    where user_id = @userId
                      and action = @action",
                    new {userId, action});
        }
    }
}
