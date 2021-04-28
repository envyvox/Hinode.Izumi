using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.CropWebService.Models;
using Hinode.Izumi.Services.WebServices.SeedWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.SeedWebService.Impl
{
    [InjectableService]
    public class SeedWebService : ISeedWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public SeedWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<SeedWebModel>> GetAllSeeds() =>
            await _con.GetConnection()
                .QueryAsync<SeedWebModel>(@"
                    select s.*, c.name as CropName, c.price as CropPrice from seeds as s
                        inner join crops c
                            on s.id = c.seed_id
                    order by s.id");

        public async Task<SeedWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<SeedWebModel>(@"
                    select s.*, c.name as CropName, c.price as CropPrice from seeds as s
                        inner join crops c
                            on s.id = c.seed_id
                    where s.id = @id",
                    new {id});

        public async Task<SeedWebModel> Update(SeedWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.SeedKey, model.Id));
            _cache.Remove(string.Format(CacheExtensions.SeedByNameKey, model.Name));
            _cache.Remove(string.Format(CacheExtensions.CropBySeedKey, model.Id));
            // обновляем семя и получаем результат
            var seed = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<SeedWebModel>(@"
                    insert into seeds(name, season, growth, re_growth, price, multiply)
                    values (@name, @season, @growth, @reGrowth, @price, @multiply)
                    on conflict (name) do update
                    set name = @name,
                        season = @season,
                        growth = @growth,
                        re_growth = @reGrowth,
                        price = @price,
                        multiply = @multiply,
                        updated_at = now()
                    returning *",
                    new
                    {
                        name = model.Name,
                        season = model.Season,
                        growth = model.Growth,
                        reGrowth = model.ReGrowth,
                        price = model.Price,
                        multiply = model.Multiply
                    });
            // обновляем урожай и получаем результат
            var crop = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CropWebModel>(@"
                    insert into crops(name, price, seed_id)
                    values (@cropName, @cropPrice, @seedId)
                    on conflict (name) do update
                        set name = @cropName,
                            price = @cropPrice,
                            seed_id = @seedId,
                            updated_at = now()
                    returning *",
                    new
                    {
                        cropName = model.CropName,
                        cropPrice = model.CropPrice,
                        seedId = seed.Id
                    });
            // возвращаем совмещенную модель
            return new SeedWebModel
            {
                    Id = seed.Id,
                    Name = seed.Name,
                    Season = seed.Season,
                    Growth = seed.Growth,
                    ReGrowth = seed.ReGrowth,
                    Price = seed.Price,
                    Multiply = seed.Multiply,
                    CreatedAt = seed.CreatedAt,
                    UpdatedAt = seed.UpdatedAt,
                    CropName = crop.Name,
                    CropPrice = crop.Price
            };
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from seeds
                    where id = @id",
                    new {id});
    }
}
