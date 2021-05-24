using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProductService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProductService.Queries
{
    public record GetAllProductsQuery : IRequest<ProductRecord[]>;

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, ProductRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllProductsHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<ProductRecord[]> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<ProductRecord>(@"
                        select * from products
                        order by id"))
                .ToArray();
        }
    }
}
