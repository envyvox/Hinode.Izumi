using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.ProductService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.ProductService.Impl
{
    [InjectableService]
    public class ProductService : IProductService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public ProductService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ProductModel[]> GetAllProducts() =>
            (await _con.GetConnection()
                .QueryAsync<ProductModel>(@"
                    select * from products
                    order by id"))
            .ToArray();

        public async Task<ProductModel> GetProduct(long id)
        {
            // проверяем продукт в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.ProductKey, id), out ProductModel product))
                return product;

            // получаем продукт из базы
            product = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ProductModel>(@"
                    select * from products
                    where id = @id",
                    new {id});

            // если такой продукт есть
            if (product != null)
            {
                // добавляем его в кэш
                _cache.Set(string.Format(CacheExtensions.ProductKey, id), product, CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return product;
            }

            // если такого продукта нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Product.Parse()));
            return new ProductModel();
        }
    }
}
