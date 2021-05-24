using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Commands
{
    public record UpdateUserInFamilyStatusCommand(long UserId, UserInFamilyStatus NewStatus) : IRequest;

    public class UpdateUserInFamilyStatusHandler : IRequestHandler<UpdateUserInFamilyStatusCommand>
    {
        private readonly IConnectionManager _con;

        public UpdateUserInFamilyStatusHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(UpdateUserInFamilyStatusCommand request, CancellationToken cancellationToken)
        {
            var (userId, newStatus) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_families
                    set status = @newStatus,
                        updated_at = now()
                    where user_id = @userId",
                    new {userId, newStatus});

            return new Unit();
        }
    }
}
