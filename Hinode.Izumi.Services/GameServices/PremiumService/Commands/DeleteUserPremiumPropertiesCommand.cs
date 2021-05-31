using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record DeleteUserPremiumPropertiesCommand(long UserId) : IRequest;

    public class DeleteUserPremiumPropertiesHandler : IRequestHandler<DeleteUserPremiumPropertiesCommand>
    {
        private readonly IConnectionManager _con;

        public DeleteUserPremiumPropertiesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(DeleteUserPremiumPropertiesCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_premium_propertieses
                    where user_id = @userId",
                    new {userId = request.UserId});

            return new Unit();
        }
    }
}
