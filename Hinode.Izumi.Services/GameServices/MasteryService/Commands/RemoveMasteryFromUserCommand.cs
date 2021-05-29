using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Commands
{
    public record RemoveMasteryFromUserCommand(long UserId, Mastery Mastery, double Amount) : IRequest;

    public class RemoveMasteryFromUserHandler : IRequestHandler<RemoveMasteryFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveMasteryFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveMasteryFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, mastery, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_masteries
                    set amount = amount - @amount,
                        updated_at = now()
                    where user_id = @userId
                      and mastery = @mastery",
                    new {userId, mastery, amount});

            return new Unit();
        }
    }
}
