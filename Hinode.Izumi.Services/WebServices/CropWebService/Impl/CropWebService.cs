using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.CropWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.CropWebService.Impl
{
    [InjectableService]
    public class CropWebService : ICropWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CropWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<CropWebModel>> GetAllCrops() =>
            await _con.GetConnection()
                .QueryAsync<CropWebModel>(@"
                    select * from crops
                    order by id");

        public async Task<CropWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropWebModel>(@"
                    select * from crops
                    where id = @id",
                    new {id});

        public async Task<CropWebModel> Upsert(CropWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.CropByIdKey, model.Id));
            _cache.Remove(string.Format(CacheExtensions.CropBySeedKey, model.SeedId));

            var query = model.Id == 0
                ? @"
                    insert into crops(name, price, seed_id)
                    values (@name, @price, @seedId)"
                : @"
                    update crops
                    set name = @name,
                        price = @price,
                        seed_id = @seedId,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropWebModel>(query,
                    new
                    {
                        id = model.Id,
                        name = model.Name,
                        price = model.Price,
                        seedId = model.SeedId
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from crops
                    where id = @id",
                    new {id});
    }
}
