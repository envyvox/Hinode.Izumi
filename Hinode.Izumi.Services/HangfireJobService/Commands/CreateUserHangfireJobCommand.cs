using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.HangfireJobService.Commands
{
    public record CreateUserHangfireJobCommand(long UserId, HangfireAction Action, string JobId) : IRequest;

    public class CreateUserHangfireJobHandler : IRequestHandler<CreateUserHangfireJobCommand>
    {
        private readonly IConnectionManager _con;

        public CreateUserHangfireJobHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateUserHangfireJobCommand request, CancellationToken cancellationToken)
        {
            var (userId, action, jobId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_hangfire_jobs(user_id, action, job_id)
                    values (@userId, @action, @jobId)",
                    new {userId, action, jobId});

            return new Unit();
        }
    }
}
