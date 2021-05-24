using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Queries
{
    public record GetUserCraftingsQuery(long UserId) : IRequest<UserCraftingRecord[]>;

    public class GetUserCraftingsHandler : IRequestHandler<GetUserCraftingsQuery, UserCraftingRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserCraftingsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCraftingRecord[]> Handle(GetUserCraftingsQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserCraftingRecord>(@"
                        select uc.*, c.name from user_craftings as uc
                            inner join craftings c
                                on c.id = uc.crafting_id
                        where uc.user_id = @userId
                        order by c.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
