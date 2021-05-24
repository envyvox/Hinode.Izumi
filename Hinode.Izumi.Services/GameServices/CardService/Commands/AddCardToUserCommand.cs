using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Commands
{
    public record AddCardToUserCommand(long UserId, long CardId) : IRequest;

    public class AddCardToUserHandler : IRequestHandler<AddCardToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddCardToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCardToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, cardId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_cards(user_id, card_id)
                    values (@userId, @cardId)
                    on conflict (user_id, card_id) do nothing",
                    new {userId, cardId});

            return new Unit();
        }
    }
}
