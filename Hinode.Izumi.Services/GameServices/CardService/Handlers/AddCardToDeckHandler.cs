using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Handlers
{
    public class AddCardToDeckHandler : IRequestHandler<AddCardToDeckCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;

        public AddCardToDeckHandler(IConnectionManager con, IMediator mediator)
        {
            _con = con;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddCardToDeckCommand request, CancellationToken cancellationToken)
        {
            var (userId, cardId) = request;
            var card = await _mediator.Send(new GetCardQuery(cardId), cancellationToken);

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_decks(user_id, card_id)
                    values (@userId, @cardId)
                    on conflict (user_id, card_id) do nothing",
                    new {userId, cardId});

            await _mediator.Send(new AddEffectToUserCommand(
                userId, card.Effect.Category(), card.Effect), cancellationToken);

            return new Unit();
        }
    }
}
