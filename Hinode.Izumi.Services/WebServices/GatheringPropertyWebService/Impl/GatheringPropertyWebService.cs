using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.WebServices.GatheringPropertyWebService.Models;
using Hinode.Izumi.Services.WebServices.GatheringWebService;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.WebServices.GatheringPropertyWebService.Impl
{
    [InjectableService]
    public class GatheringPropertyWebService : IGatheringPropertyWebService
    {
        private readonly IConnectionManager _con;
        private readonly IGatheringWebService _gatheringWebService;
        private readonly IMemoryCache _cache;

        public GatheringPropertyWebService(IConnectionManager con, IGatheringWebService gatheringWebService,
            IMemoryCache cache)
        {
            _con = con;
            _gatheringWebService = gatheringWebService;
            _cache = cache;
        }

        public async Task<IEnumerable<GatheringPropertyWebModel>> GetAllGatheringProperties() =>
            await _con.GetConnection()
                .QueryAsync<GatheringPropertyWebModel>(@"
                    select gp.*, g.name as GatheringName from gathering_properties as gp
                        inner join gatherings g
                            on g.id = gp.gathering_id
                    order by gp.gathering_id");

        public async Task<GatheringPropertyWebModel> Get(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringPropertyWebModel>(@"
                    select gp.*, g.name as GatheringName from gathering_properties as gp
                        inner join gatherings g
                            on g.id = gp.gathering_id
                    where gp.id = @id",
                    new {id});

        public async Task<GatheringPropertyWebModel> Update(GatheringPropertyWebModel model)
        {
            // сбрасываем кэш
            _cache.Remove(string.Format(CacheExtensions.GatheringPropertyKey, model.Id, model.Property));
            // обновляем базу
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<GatheringPropertyWebModel>(@"
                    update gathering_properties
                    set mastery0 = @mastery0,
                        mastery50 = @mastery50,
                        mastery100 = @mastery100,
                        mastery150 = @mastery150,
                        mastery200 = @mastery200,
                        mastery250 = @mastery250,
                        updated_at = now()
                    where id = @id
                    returning *",
                    new
                    {
                        id = model.Id,
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
            // получаем все собирательские предметы
            var gatherings = await _gatheringWebService.GetAllGathering();
            // получаем все свойства собирательского предмета
            var gatheringProperties = Enum.GetValues(typeof(GatheringProperty))
                .Cast<GatheringProperty>()
                .Select(gatheringProperty => (long) gatheringProperty.GetHashCode())
                .ToArray();

            // для каждого собирательского предмета добавляем в базу свойства с значением по-умолчанию
            foreach (var gathering in gatherings)
            {
                await _con.GetConnection()
                    .ExecuteAsync(@"
                    insert into gathering_properties(gathering_id, property)
                    values (@gatheringId, unnest(array[@gatheringProperties]))
                    on conflict (gathering_id, property) do nothing",
                        new {gatheringId = gathering.Id, gatheringProperties});
            }
        }
    }
}
