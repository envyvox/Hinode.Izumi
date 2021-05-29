using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CollectionService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CollectionService.Queries
{
    public record GetUserCollectionsQuery(
            long UserId,
            CollectionCategory Category,
            Event Event = Event.None)
        : IRequest<UserCollectionRecord[]>;

    public class GetUserCollectionsHandler : IRequestHandler<GetUserCollectionsQuery, UserCollectionRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserCollectionsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCollectionRecord[]> Handle(GetUserCollectionsQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, category, @event) = request;
            return (await _con.GetConnection()
                    .QueryAsync<UserCollectionRecord>(@"
                        select * from user_collections
                        where user_id = @userId
                          and category = @category
                          and event = @event",
                        new {userId, category, @event}))
                .ToArray();
        }
    }
}
