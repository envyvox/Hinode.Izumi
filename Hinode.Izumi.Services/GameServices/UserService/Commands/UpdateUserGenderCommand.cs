using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.UserService.Commands
{
    public record UpdateUserGenderCommand(long Id, Gender Gender) : IRequest;

    public class UpdateUserGenderHandler : IRequestHandler<UpdateUserGenderCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserGenderHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserGenderCommand request, CancellationToken cancellationToken)
        {
            var (userId, gender) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update users
                    set gender = @gender,
                        updated_at = now()
                    where id = @userId",
                    new {userId, gender});

            return new Unit();
        }
    }
}
