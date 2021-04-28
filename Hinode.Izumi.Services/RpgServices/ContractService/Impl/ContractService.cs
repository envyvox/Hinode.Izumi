using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.ContractService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.ContractService.Impl
{
    [InjectableService]
    public class ContractService : IContractService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public ContractService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ContractModel> GetContract(long id)
        {
            // проверяем рабочий контракт в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.ContractKey, id), out ContractModel contract))
                return contract;

            // получаем рабочий контракт из базы
            contract = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractModel>(@"
                    select * from contracts
                    where id = @id",
                    new {id});

            // если рабочий контракт с таким id есть
            if (contract != null)
            {
                // добавляем его в кэш
                _cache.Set(string.Format(CacheExtensions.ContractKey, id), contract,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return contract;
            }

            // если такого рабочего контракта нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.ContractById.Parse()));
            return new ContractModel();
        }

        public async Task<ContractModel[]> GetContract(Location location)
        {
            // проверяем рабочие контракты в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.ContractLocationKey, location),
                out ContractModel[] contracts))
                return contracts;

            // получаем рабочие контракты из базы
            contracts = (await _con.GetConnection()
                    .QueryAsync<ContractModel>(@"
                        select * from contracts
                        where location = @location",
                        new {location}))
                .ToArray();

            // если рабочие контракты в этой локации есть
            if (contracts.Length > 0)
            {
                // добавляем их в кэш
                _cache.Set(string.Format(CacheExtensions.ContractLocationKey, location), contracts,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return contracts;
            }

            // если таких рабочих контрактов нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.ContractByLocation.Parse()));
            return Array.Empty<ContractModel>();
        }

        public async Task<ContractModel> GetUserContract(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractModel>(@"
                    select c.* from user_contracts as uc
                        inner join contracts c
                            on c.id = uc.contract_id
                    where uc.user_id = @userId",
                    new {userId});

        public async Task AddContractToUser(long userId, long contractId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_contracts(user_id, contract_id)
                    values (@userId, @contractId)
                    on conflict (user_id) do nothing",
                    new {userId, contractId});

        public async Task RemoveContractFromUser(long userId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_contracts
                    where user_id = @userId",
                    new {userId});
    }
}
