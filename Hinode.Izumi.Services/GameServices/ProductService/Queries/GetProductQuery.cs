using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProductService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.ProductService.Queries
{
    public record GetProductQuery(long Id) : IRequest<ProductRecord>;

    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetProductHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ProductRecord> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.ProductKey, request.Id), out ProductRecord product))
                return product;

            product = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ProductRecord>(@"
                    select * from products
                    where id = @id",
                    new {id = request.Id});

            if (product is not null)
            {
                _cache.Set(string.Format(CacheExtensions.ProductKey, request.Id), product,
                    CacheExtensions.DefaultCacheOptions);
                return product;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Product.Parse()));
            return null;
        }
    }
}
