using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.PremiumService.Commands
{
    public record CreateUserPremiumPropertiesCommand(long UserId) : IRequest;

    public class CreateUserPremiumPropertiesHandler : IRequestHandler<CreateUserPremiumPropertiesCommand>
    {
        private readonly IConnectionManager _con;

        public CreateUserPremiumPropertiesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(CreateUserPremiumPropertiesCommand request, CancellationToken cancellationToken)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_premium_propertieses(user_id)
                    values (@userId)
                    on conflict (user_id) do nothing",
                    new {userId = request.UserId});

            return new Unit();
        }
    }
}
