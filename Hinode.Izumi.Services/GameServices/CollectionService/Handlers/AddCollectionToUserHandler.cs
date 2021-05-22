using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CollectionService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Handlers
{
    public class AddCollectionToUserHandler : IRequestHandler<AddCollectionToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddCollectionToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCollectionToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, category, itemId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_collections(user_id, category, item_id)
                    values (@userId, @category, @itemId)
                    on conflict (user_id, category, item_id) do nothing",
                    new {userId, category, itemId});

            return new Unit();
        }
    }
}
