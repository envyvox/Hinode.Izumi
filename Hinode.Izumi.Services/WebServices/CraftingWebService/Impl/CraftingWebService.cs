using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.CraftingWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.CraftingWebService.Impl
{
    [InjectableService]
    public class CraftingWebService : ICraftingWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CraftingWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<CraftingWebModel>> GetAllCrafting() =>
            await _con.GetConnection()
                .QueryAsync<CraftingWebModel>(@"
                    select * from craftings
                    order by id");

        public async Task<CraftingWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingWebModel>(@"
                    select * from craftings
                    where id = @id",
                    new {id});

        public async Task<CraftingWebModel> Upsert(CraftingWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.CraftingKey, model.Id));

            var query = model.Id == 0
                ? @"
                    insert into craftings(name, time, location)
                    values (@name, @time, @location)
                    returning *"
                : @"
                    update craftings
                    set name = @name,
                        time = @time,
                        location = @location,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingWebModel>(query,
                    new
                    {
                        id = model.Id,
                        name = model.Name,
                        time = model.Time,
                        location = model.Location
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from craftings
                    where id = @id",
                    new {id});
    }
}
