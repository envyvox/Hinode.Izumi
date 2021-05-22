using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReputationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Handlers
{
    public class AddReputationToUserHandler : IRequestHandler<AddReputationToUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddReputationToUserHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddReputationToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, reputation, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (@userId, @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.amount + @amount,
                            updated_at = now()",
                    new {userId, reputation, amount});

            await _mediator.Send(new CheckReputationRewardsCommand(userId, reputation), cancellationToken);

            return new Unit();
        }
    }
}
