using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.MarketService.Models;

namespace Hinode.Izumi.Services.RpgServices.MarketService.Impl
{
    [InjectableService]
    public class MarketService : IMarketService
    {
        private readonly IConnectionManager _con;
        private readonly ILocalizationService _local;

        public MarketService(IConnectionManager con, ILocalizationService local)
        {
            _con = con;
            _local = local;
        }

        public async Task<MarketRequestModel> GetMarketRequest(long requestId)
        {
            // получаем заявку из базы
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MarketRequestModel>(@"
                    select * from market_requests
                    where id = @requestId",
                    new {requestId});

            // если такая заявка есть - возвращаем ее
            if (res != null) return res;

            // если нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.MarketRequest.Parse()));
            return new MarketRequestModel();
        }

        public async Task<MarketRequestModel[]> GetMarketRequest(MarketCategory category, bool selling,
            string namePattern = null)
        {
            long? itemId = namePattern == null
                // если название пустое, то нам не нужно искать заявки определенного предмета
                ? null
                // если не пустое - нужно получить id предмета, который мы будем искать на рынке
                : (await _local.GetLocalizationByLocalizedWord(category, namePattern)).ItemId;

            // возвращаем подходящие заявки
            return (await _con.GetConnection()
                    .QueryAsync<MarketRequestModel>(@"
                        select * from market_requests
                        where category = @category
                          and (
                              @itemId is null
                                  or item_id = @itemId
                              )
                          and selling = @selling
                        order by price desc
                        limit 5",
                        new {category, itemId, selling}))
                .ToArray();
        }

        public async Task<MarketRequestModel> GetMarketUserRequest(long userId, MarketCategory category, long itemId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MarketRequestModel>(@"
                    select * from market_requests
                    where user_id = @userId
                      and category = @category
                      and item_id = @itemId
                    order by id",
                    new {userId, category, itemId});

        public async Task<MarketRequestModel[]> GetMarketUserRequest(long userId, MarketCategory category) =>
            (await _con.GetConnection()
                .QueryAsync<MarketRequestModel>(@"
                    select * from market_requests
                    where user_id = @userId
                      and category = @category
                    order by id",
                    new {userId, category}))
            .ToArray();

        public async Task<MarketRequestModel[]> GetMarketUserRequest(long userId) =>
            (await _con
                .GetConnection()
                .QueryAsync<MarketRequestModel>(@"
                    select * from market_requests
                    where user_id = @userId
                    order by id",
                    new {userId}))
            .ToArray();

        public async Task<bool> CheckMarketUserRequest(long userId, MarketCategory category, long itemId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from market_requests
                    where user_id = @userId
                      and category = @category
                      and item_id = @itemId",
                    new {userId, category, itemId});

        public async Task AddOrUpdateMarketRequest(long userId, MarketCategory category, long itemId, long price,
            long amount, bool selling) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into market_requests(user_id, category, item_id, price, amount, selling)
                    values (@userId, @category, @itemId, @price, @amount, @selling)
                    on conflict (user_id, category, item_id) do update
                        set price = @price,
                            amount = @amount,
                            updated_at = now()",
                    new {userId, category, itemId, price, amount, selling});

        public async Task UpdateOrDeleteMarketRequest(MarketCategory category, long itemId, long marketAmount,
            long amount)
        {
            var query = marketAmount == amount
                // если это был последний товар, то заявку нужно удалить
                ? @"delete from market_requests
                        where category = @category
                          and item_id = @itemId"
                // если нет - обновить
                : @"update market_requests
                        set amount = amount - @amount,
                            updated_at = now()
                    where category = @category
                      and item_id = @itemId";

            await _con.GetConnection()
                .ExecuteAsync(query, new {category, itemId, amount});
        }

        public async Task DeleteMarketRequest(long requestId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from market_requests
                    where id = @requestId",
                    new {requestId});
    }
}
