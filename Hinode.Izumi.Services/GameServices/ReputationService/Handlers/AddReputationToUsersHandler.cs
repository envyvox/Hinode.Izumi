using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ReputationService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Handlers
{
    public class AddReputationToUsersHandler : IRequestHandler<AddReputationToUsersCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddReputationToUsersHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddReputationToUsersCommand request, CancellationToken cancellationToken)
        {
            var (usersId, reputation, amount) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputations as ur (user_id, reputation, amount)
                    values (unnest(array[@usersId]), @reputation, @amount)
                    on conflict (user_id, reputation) do update
                        set amount = ur.reputation + @reputation,
                            updated_at = now()",
                    new {usersId, reputation, amount});

            foreach (var userId in usersId)
                await _mediator.Send(new CheckReputationRewardsCommand(userId, reputation), cancellationToken);

            return new Unit();
        }
    }
}
