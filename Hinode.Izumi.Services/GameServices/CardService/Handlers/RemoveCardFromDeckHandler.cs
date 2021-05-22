using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Handlers
{
    public class RemoveCardFromDeckHandler : IRequestHandler<RemoveCardFromDeckCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public RemoveCardFromDeckHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RemoveCardFromDeckCommand request, CancellationToken cancellationToken)
        {
            var (userId, cardId) = request;
            var card = await _mediator.Send(new GetCardQuery(cardId), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_decks
                    where user_id = @userId
                      and card_id = @cardId",
                    new {userId, cardId});

            await _mediator.Send(new RemoveEffectFromUserCommand(userId, card.Effect), cancellationToken);

            return new Unit();
        }
    }
}
