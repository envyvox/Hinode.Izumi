using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.TransitWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.TransitWebService.Impl
{
    [InjectableService]
    public class TransitWebService : ITransitWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public TransitWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<TransitWebModel>> GetAllTransits() =>
            await _con.GetConnection()
                .QueryAsync<TransitWebModel>(@"
                    select * from transits
                    order by id");

        public async Task<TransitWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TransitWebModel>(@"
                    select * from transits
                    where id = @id",
                    new {id});

        public async Task<TransitWebModel> Upsert(TransitWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.TransitKey, model.Departure, model.Destination));
            _cache.Remove(string.Format(CacheExtensions.TransitsLocationKey, model.Departure));
            _cache.Remove(string.Format(CacheExtensions.TransitsLocationKey, model.Destination));

            var query = model.Id == 0
                ? @"
                    insert into transits(departure, destination, time, price)
                    values (@departure, @destination, @time, @price)
                    returning *"
                : @"
                    update transits
                    set time = @time,
                        price = @price,
                        updated_at = now()
                    where id = @id
                    returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<TransitWebModel>(query,
                    new
                    {
                        id = model.Id,
                        departure = model.Departure,
                        destination = model.Destination,
                        time = model.Time,
                        price = model.Price
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from transits
                    where id = @id",
                    new {id});
    }
}
