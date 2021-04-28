using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CollectionService.Models;

namespace Hinode.Izumi.Services.RpgServices.CollectionService.Impl
{
    [InjectableService]
    public class CollectionService : ICollectionService
    {
        private readonly IConnectionManager _con;

        public CollectionService(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCollectionModel[]> GetUserCollection(long userId, CollectionCategory category) =>
            (await _con.GetConnection()
                .QueryAsync<UserCollectionModel>(@"
                    select * from user_collections
                    where user_id = @userId
                      and category = @category",
                    new {userId, category}))
            .ToArray();

        public async Task AddCollectionToUser(long userId, CollectionCategory category, long itemId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_collections(user_id, category, item_id)
                    values (@userId, @category, @itemId)
                    on conflict (user_id, category, item_id) do nothing",
                    new {userId, category, itemId});
    }
}
