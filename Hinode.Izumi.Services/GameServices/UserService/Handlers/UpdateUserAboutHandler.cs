using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Handlers
{
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
