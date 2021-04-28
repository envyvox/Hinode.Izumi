using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.PropertyService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.PropertyService.Impl
{
    [InjectableService]
    public class PropertyService : IPropertyService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public PropertyService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<long> GetPropertyValue(Property property)
        {
            // проверяем свойство в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.PropertyKey, property), out long value)) return value;

            // получаем свойство из базы
            value = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<long>(@"
                    select value from world_properties
                    where property = @property",
                    new {property});

            // добавляем свойство в кэш
            _cache.Set(string.Format(CacheExtensions.PropertyKey, property), value,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойство
            return value;
        }

        public async Task<MasteryPropertyModel> GetMasteryProperty(MasteryProperty property)
        {
            // проверяем свойства в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.MasteryPropertyKey, property),
                out MasteryPropertyModel properties))
                return properties;

            // получаем свойства из базы
            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryPropertyModel>(@"
                    select * from mastery_properties
                    where property = @property",
                    new {property});

            // добавляем свойства в кэш
            _cache.Set(string.Format(CacheExtensions.MasteryPropertyKey, property), properties,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойства
            return properties;
        }

        public async Task<MasteryXpPropertyModel> GetMasteryXpProperty(MasteryXpProperty property)
        {
            // проверяем свойства в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.MasteryXpPropertyKey, property),
                out MasteryXpPropertyModel properties))
                return properties;

            // получаем свойства из базы
            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryXpPropertyModel>(@"
                    select * from mastery_xp_properties
                    where property = @property",
                    new {property});

            // добавляем свойства в кэш
            _cache.Set(string.Format(CacheExtensions.MasteryXpPropertyKey, property), properties,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойства
            return properties;
        }

        public async Task<GatheringPropertyModel> GetGatheringProperty(long gatheringId, GatheringProperty property)
        {
            // проверяем свойства в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.GatheringPropertyKey, gatheringId, property),
                out GatheringPropertyModel properties)) return properties;

            // получаем свойства из базы
            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringPropertyModel>(@"
                    select * from gathering_properties
                    where gathering_id = @gatheringId
                      and property = @property",
                    new {gatheringId, property});

            // добавляем свойства в кэш
            _cache.Set(string.Format(CacheExtensions.GatheringPropertyKey, gatheringId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойства
            return properties;
        }

        public async Task<CraftingPropertyModel> GetCraftingProperty(long craftingId, CraftingProperty property)
        {
            // проверяем свойства в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CraftingPropertyKey, craftingId, property),
                out CraftingPropertyModel properties)) return properties;

            // получаем свойства из базы
            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CraftingPropertyModel>(@"
                    select * from crafting_properties
                    where crafting_id = @craftingId
                      and property = @property",
                    new {craftingId, property});

            // добавляем свойства в кэш
            _cache.Set(string.Format(CacheExtensions.CraftingPropertyKey, craftingId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойства
            return properties;
        }

        public async Task<AlcoholPropertyModel> GetAlcoholProperty(long alcoholId, AlcoholProperty property)
        {
            // проверяем свойства в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.AlcoholPropertyKey, alcoholId, property),
                out AlcoholPropertyModel properties)) return properties;

            // получаем свойства из базы
            properties = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholPropertyModel>(@"
                    select * from alcohol_properties
                    where alcohol_id = @alcoholId
                      and property = @property",
                    new {property});

            // добавляем свойства в кэш
            _cache.Set(string.Format(CacheExtensions.AlcoholPropertyKey, alcoholId, property), properties,
                CacheExtensions.DefaultCacheOptions);

            // возвращаем свойства
            return properties;
        }

        public async Task UpdateProperty(Property property, long newValue) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update world_properties
                    set value = @newValue,
                        updated_at = now()
                    where property = @property",
                    new {property, newValue});
    }
}
