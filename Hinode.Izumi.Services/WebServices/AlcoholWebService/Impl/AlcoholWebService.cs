using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.AlcoholWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.AlcoholWebService.Impl
{
    [InjectableService]
    public class AlcoholWebService : IAlcoholWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public AlcoholWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<AlcoholWebModel>> GetAllAlcohols() =>
            await _con.GetConnection()
                .QueryAsync<AlcoholWebModel>(@"
                    select * from alcohols
                    order by id");

        public async Task<AlcoholWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholWebModel>(@"
                    select * from alcohols
                    where id = @id",
                    new {id});

        public async Task<AlcoholWebModel> Upsert(AlcoholWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.AlcoholKey, model.Id));

            var query = model.Id == 0
                ? @"
                    insert into alcohols(name, time)
                    values (@name, @time)
                    returning *"
                : @"
                    update alcohols
                    set name = @name,
                        time = @time,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholWebModel>(query,
                    new
                    {
                        id = model.Id,
                        name = model.Name,
                        time = model.Time
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from alcohols
                    where id = @id",
                    new {id});
    }
}
