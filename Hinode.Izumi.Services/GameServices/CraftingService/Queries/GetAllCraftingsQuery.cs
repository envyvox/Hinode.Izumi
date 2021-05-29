using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CraftingService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CraftingService.Queries
{
    public record GetAllCraftingsQuery : IRequest<CraftingRecord[]>;

    public class GetAllCraftingsHandler : IRequestHandler<GetAllCraftingsQuery, CraftingRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllCraftingsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CraftingRecord[]> Handle(GetAllCraftingsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CraftingRecord>(@"
                        select * from craftings"))
                .ToArray();
        }
    }
}
