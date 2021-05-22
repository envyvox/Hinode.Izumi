using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserCropsHandler : IRequestHandler<GetUserCropsQuery, UserCropRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetUserCropsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<UserCropRecord[]> Handle(GetUserCropsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<UserCropRecord>(@"
                        select uc.*, c.name, s.season from user_crops as uc
                            inner join crops c
                                on c.id = uc.crop_id
                            inner join seeds s
                                on s.id = c.seed_id
                        where uc.user_id = @userId
                        order by c.id",
                        new {userId = request.UserId}))
                .ToArray();
        }
    }
}
