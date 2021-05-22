using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.MasteryService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Handlers
{
    public class AddMasteryToUserHandler : IRequestHandler<AddMasteryToUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddMasteryToUserHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddMasteryToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, mastery, amount) = request;
            var userMaxMastery = await _mediator.Send(new GetUserMaxMasteryQuery(userId), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_masteries as um (user_id, mastery, amount)
                    values (@userId, @mastery, @amount)
                    on conflict (user_id, mastery) do update
                        set amount = (
                            case when um.amount + @amount <= @userMaxMastery then um.amount + @amount
                                 else @userMaxMastery
                            end),
                            updated_at = now()",
                    new {userId, mastery, amount, userMaxMastery});

            return new Unit();
        }
    }
}
