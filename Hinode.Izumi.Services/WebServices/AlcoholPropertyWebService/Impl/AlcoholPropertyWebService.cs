using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService.Impl
{
    [InjectableService]
    public class AlcoholPropertyWebService : IAlcoholPropertyWebService
    {
        private readonly IConnectionManager _con;
        private readonly IAlcoholService _alcoholService;
        private readonly IMemoryCache _cache;

        public AlcoholPropertyWebService(IConnectionManager con, IAlcoholService alcoholService, IMemoryCache cache)
        {
            _con = con;
            _alcoholService = alcoholService;
            _cache = cache;
        }

        public async Task<IEnumerable<AlcoholPropertyWebModel>> GetAllAlcoholProperties() =>
            await _con.GetConnection()
                .QueryAsync<AlcoholPropertyWebModel>(@"
                    select ap.*, a.name as AlcoholName from alcohol_properties as ap
                        inner join alcohols a
                            on a.id = ap.alcohol_id
                    order by ap.alcohol_id");

        public async Task<IEnumerable<AlcoholPropertyWebModel>> GetAlcoholProperties(long alcoholId) =>
            await _con.GetConnection()
                .QueryAsync<AlcoholPropertyWebModel>(@"
                    select ap.*, a.name as AlcoholName from alcohol_properties as ap
                        inner join alcohols a
                            on a.id = ap.alcohol_id
                    where ap.alcohol_id = @alcoholId",
                    new {alcoholId});

        public async Task<AlcoholPropertyWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholPropertyWebModel>(@"
                    select ap.*, a.name as AlcoholName from alcohol_properties as ap
                        inner join alcohols a
                            on a.id = ap.alcohol_id
                    where ap.id = @id",
                    new {id});

        public async Task<AlcoholPropertyWebModel> Update(AlcoholPropertyWebModel model)
        {
            // сбрасываем запись в кэше
            _cache.Remove(string.Format(CacheExtensions.AlcoholPropertyKey, model.AlcoholId, model.Property));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<AlcoholPropertyWebModel>(@"
                    insert into alcohol_properties(alcohol_id, property, mastery0, mastery50, mastery100, mastery150, mastery200, mastery250)
                    values (@alcoholId, @property, @mastery0, @mastery50, @mastery100, @mastery150, @mastery200, @mastery250)
                    on conflict (alcohol_id, property) do update
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
                        alcoholId = model.AlcoholId,
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
            // получаем весь алкоголь
            var alcohols = await _alcoholService.GetAllAlcohol();
            // получаем все свойства алкоголя в массив номеров
            var alcoholProperties = Enum.GetValues(typeof(AlcoholProperty))
                .Cast<AlcoholProperty>()
                .Select(x => (long) x.GetHashCode())
                .ToArray();

            // для каждого алкоголя добавляем его свойства в базу с значениями по-умолчанию
            foreach (var alcohol in alcohols)
            {
                await _con.GetConnection()
                    .ExecuteAsync(@"
                        insert into alcohol_properties(alcohol_id, property)
                        values (@alcoholId, unnest(array[@alcoholProperties]))
                        on conflict (alcohol_id, property) do nothing",
                        new {alcoholId = alcohol.Id, alcoholProperties});
            }
        }
    }
}
