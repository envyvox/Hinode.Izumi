using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.MasteryPropertyWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.MasteryPropertyWebService.Impl
{
    [InjectableService]
    public class MasteryPropertyWebService : IMasteryPropertyWebService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public MasteryPropertyWebService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<IEnumerable<MasteryPropertyWebModel>> GetAllProperties() =>
            await _con.GetConnection()
                .QueryAsync<MasteryPropertyWebModel>(@"
                    select * from mastery_properties
                    order by id");

        public async Task<MasteryPropertyWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryPropertyWebModel>(@"
                    select * from mastery_properties
                    where id = @id",
                    new {id});

        public async Task<MasteryPropertyWebModel> Update(MasteryPropertyWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.MasteryPropertyKey, model.Property));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<MasteryPropertyWebModel>(@"
                    insert into mastery_properties(property_category, property, mastery0, mastery50, mastery100, mastery150, mastery200, mastery250)
                    values (@propertyCategory, @property, @mastery0, @mastery50, @mastery100, @mastery150, @mastery200, @mastery250)
                    on conflict (property) do update
                    set mastery0 = @mastery0,
                        mastery50 = @mastery50,
                        mastery100 = @mastery100,
                        mastery150 = @mastery150,
                        mastery200 = @mastery200,
                        mastery250 = @mastery250,
                        updated_at = now()
                    returning *",
                    new
                    {
                        propertyCategory = model.PropertyCategory,
                        property = model.Property,
                        mastery0 = model.Mastery0,
                        mastery50 = model.Mastery50,
                        mastery100 = model.Mastery100,
                        mastery150 = model.Mastery150,
                        mastery200 = model.Mastery200,
                        mastery250 = model.Mastery250
                    });
        }

        public async Task Upload()
        {
            // получаем категории свойств
            var propertyCategories = new List<long>();
            // получаем свойства
            var properties = new List<long>();
            foreach (var masteryProperty in Enum.GetValues(typeof(MasteryProperty)).Cast<MasteryProperty>())
            {
                propertyCategories.Add(masteryProperty.Category().GetHashCode());
                properties.Add(masteryProperty.GetHashCode());
            }

            // добавляем их в базу
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into mastery_properties(property_category, property)
                    values (unnest(array[@propertyCategories]), unnest(array[@properties]))
                    on conflict (property) do nothing",
                    new {propertyCategories, properties});
        }
    }
}
