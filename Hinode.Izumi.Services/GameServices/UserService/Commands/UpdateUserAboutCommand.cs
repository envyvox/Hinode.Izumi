using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserAboutCommand(long Id, string NewAbout) : IRequest;

    public class UpdateUserAboutHandler : IRequestHandler<UpdateUserAboutCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserAboutHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserAboutCommand request, CancellationToken cancellationToken)
        {
            var (userId, about) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set about = @about,
                        updated_at = now()
                    where id = @userId",
                    new {userId, about});

            return new Unit();
        }
    }
}
