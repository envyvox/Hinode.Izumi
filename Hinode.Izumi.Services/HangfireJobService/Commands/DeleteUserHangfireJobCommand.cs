using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.HangfireJobService.Commands
{
    public record DeleteUserHangfireJobCommand(long UserId, HangfireAction Action) : IRequest;

    public class DeleteUserHangfireJobHandler : IRequestHandler<DeleteUserHangfireJobCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteUserHangfireJobHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteUserHangfireJobCommand request, CancellationToken cancellationToken)
        {
            var (userId, action) = request;
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_hangfire_jobs
                    where user_id = @userId
                      and action = @action",
                    new {userId, action});

            return new Unit();
        }
    }
}
