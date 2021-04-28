using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.ContractWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.ContractWebService.Impl
{
    [InjectableService]
    public class ContractWebService : IContractWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public ContractWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<ContractWebModel>> GetAllContracts() =>
            await _con.GetConnection()
                .QueryAsync<ContractWebModel>(@"
                    select * from contracts
                    order by id");

        public async Task<ContractWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractWebModel>(@"
                    select * from contracts
                    where id = @id",
                    new {id});

        public async Task<ContractWebModel> Upsert(ContractWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.ContractKey, model.Id));

            var query = model.Id == 0
                ? @"
                    insert into contracts(location, name, description, time, currency, reputation, energy)
                    values (@location, @name, @description, @time, @currency, @reputation, @energy)
                    returning *"
                : @"
                    update contracts
                    set location = @location,
                        description = @description,
                        time = @time,
                        currency = @currency,
                        reputation = @reputation,
                        energy = @energy,
                        updated_at = now()
                   where id = @id
                   returning *";

            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractWebModel>(query,
                    new
                    {
                        id = model.Id,
                        location = model.Location,
                        name = model.Name,
                        description = model.Description,
                        time = model.Time,
                        currency = model.Currency,
                        reputation = model.Reputation,
                        energy = model.Energy
                    });
        }

        public async Task Remove(long id) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from contracts
                    where id = @id",
                    new {id});
    }
}
