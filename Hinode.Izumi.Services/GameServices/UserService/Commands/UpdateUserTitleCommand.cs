using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserTitleCommand(long Id, Title Title) : IRequest;

    public class UpdateUserTitleHandler : IRequestHandler<UpdateUserTitleCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserTitleHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserTitleCommand request, CancellationToken cancellationToken)
        {
            var (userId, title) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set title = @title,
                        updated_at = now()
                    where id = @userId",
                    new {userId, title});

            return new Unit();
        }
    }
}
