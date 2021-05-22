using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CollectionService.Queries;
using Hinode.Izumi.Services.GameServices.CollectionService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Handlers
{
    public class GetUserCollectionHandler : IRequestHandler<GetUserCollectionQuery, UserCollectionRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserCollectionHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCollectionRecord[]> Handle(GetUserCollectionQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, category) = request;
            return (await _con.GetConnection()
                    .QueryAsync<UserCollectionRecord>(@"
                        select * from user_collections
                        where user_id = @userId
                          and category = @category",
                        new {userId, category}))
                .ToArray();
        }
    }
}
