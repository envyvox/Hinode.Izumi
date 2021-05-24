using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserBoxesQuery(long UserId) : IRequest<Dictionary<Box, UserBoxRecord>>;

    public class GetUserBoxesHandler : IRequestHandler<GetUserBoxesQuery, Dictionary<Box, UserBoxRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserBoxesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Box, UserBoxRecord>> Handle(GetUserBoxesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserBoxRecord>(@"
                        select * from user_boxes
                        where user_id = @userId
                        order by box",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Box);
        }
    }
}
