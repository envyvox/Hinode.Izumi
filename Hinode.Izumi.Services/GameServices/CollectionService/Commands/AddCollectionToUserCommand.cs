using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Commands
{
    public record AddCollectionToUserCommand(
            long UserId,
            CollectionCategory Category,
            long ItemId,
            Event Event = Event.None)
        : IRequest;

    public class AddCollectionToUserHandler : IRequestHandler<AddCollectionToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddCollectionToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCollectionToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, category, itemId, @event) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_collections(user_id, category, item_id, event)
                    values (@userId, @category, @itemId, @event)
                    on conflict (user_id, category, item_id) do nothing",
                    new {userId, category, itemId, @event});

            return new Unit();
        }
    }
}
