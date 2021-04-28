using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.WorldPropertyWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.WorldPropertyWebService.Impl
{
    [InjectableService]
    public class WorldPropertyWebService : IWorldPropertyWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public WorldPropertyWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<WorldPropertyWebModel>> GetAllProperties() =>
            await _con.GetConnection()
                .QueryAsync<WorldPropertyWebModel>(@"
                    select * from world_properties
                    order by id");

        public async Task<WorldPropertyWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<WorldPropertyWebModel>(@"
                    select * from world_properties
                    where id = @id",
                    new {id});

        public async Task<WorldPropertyWebModel> Update(WorldPropertyWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.PropertyKey, model.Property));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<WorldPropertyWebModel>(@"
                    insert into world_properties(property_category, property, value)
                    values (@propertyCategory, @property, @value)
                    on conflict (property) do update
                    set value = @value,
                        updated_at = now()
                    returning *",
                    new
                    {
                        propertyCategory = model.PropertyCategory,
                        property = model.Property,
                        value = model.Value
                    });
        }

        public async Task Upload()
        {
            // получаем категории свойств
            var propertyCategories = new List<long>();
            // получаем свойства
            var properties = new List<long>();
            foreach (var property in Enum.GetValues(typeof(Property)).Cast<Property>())
            {
                propertyCategories.Add(property.Category().GetHashCode());
                properties.Add(property.GetHashCode());
            }

            // добавляем свойства в базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into world_properties(property_category, property, value)
                    values (unnest(array[@propertyCategories]), unnest(array[@properties]), 0)
                    on conflict (property) do nothing",
                    new {propertyCategories, properties});
        }
    }
}
