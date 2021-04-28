using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.BuildingService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.BuildingService.Impl
{
    [InjectableService]
    public class BuildingService : IBuildingService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public BuildingService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<BuildingModel> GetBuilding(long buildingId)
        {
            // проверяем постройку в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.BuildingIdKey, buildingId),
                out BuildingModel building))
                return building;

            // получаем постройку из базы
            building = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BuildingModel>(@"
                    select * from buildings
                    where id = @buildingId",
                    new {buildingId});

            // если такая постройка есть
            if (building != null)
            {
                // добавляем ее в кэш
                _cache.Set(string.Format(CacheExtensions.BuildingIdKey, buildingId), building,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return building;
            }

            // если такой постройки нет - вывошим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Building.Parse()));
            return new BuildingModel();
        }

        public async Task<BuildingModel> GetBuilding(Building type)
        {
            // проверяем постройку в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.BuildingTypeKey, type), out BuildingModel building))
                return building;

            // получаем постройку из базы
            building = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BuildingModel>(@"
                    select * from buildings
                    where type = @type",
                    new {type});

            // если такая постройка есть
            if (building != null)
            {
                // добавляем ее в кэш
                _cache.Set(string.Format(CacheExtensions.BuildingTypeKey, type), building,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return building;
            }

            // если такой постройки нет - вывошим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Building.Parse()));
            return new BuildingModel();
        }

        public async Task<BuildingModel> GetBuildingByProjectId(long projectId)
        {
            // проверяем постройку в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.BuildingByProjectKey, projectId),
                out BuildingModel building))
                return building;

            // получаем постройку из базы
            building = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BuildingModel>(@"
                    select * from buildings
                    where project_id = @projectId",
                    new {projectId});

            // если такая постройка есть
            if (building != null)
            {
                // добавляем ее в кэш
                _cache.Set(string.Format(CacheExtensions.BuildingByProjectKey, projectId), building,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return building;
            }

            // если такой постройки нет - вывошим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Building.Parse()));
            return new BuildingModel();
        }

        public async Task<BuildingModel[]> GetUserBuildings(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<BuildingModel>(@"
                    select b.* from user_buildings as ub
                        inner join buildings b
                            on b.id = ub.building_id
                    where ub.user_id = @userId
                    order by ub.building_id",
                    new {userId}))
            .ToArray();

        public async Task<BuildingModel[]> GetFamilyBuildings(long familyId) =>
            (await _con.GetConnection()
                .QueryAsync<BuildingModel>(@"
                    select b.* from family_buildings as fb
                        inner join buildings b
                            on b.id = fb.building_id
                    where fb.family_id = @familyId
                    order by fb.building_id",
                    new {familyId}))
            .ToArray();

        public async Task<bool> CheckBuildingInUser(long userId, Building type) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_buildings
                    where user_id = @userId
                      and building_id = (
                          select id from buildings
                          where type = @type
                          )",
                    new {userId, type});

        public async Task<bool> CheckBuildingInFamily(long familyId, Building type) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from family_buildings
                    where family_id = @familyId
                      and building_id = (
                          select id from buildings
                          where type = @type
                      )",
                    new {familyId, type});

        public async Task AddBuildingToUser(long userId, Building type) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_buildings(user_id, building_id)
                    values (@userId, (
                        select id from buildings
                        where type = @type
                    ))
                    on conflict (user_id, building_id) do nothing",
                    new {userId, type});

        public async Task AddBuildingToFamily(long familyId, Building type) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_buildings(family_id, building_id)
                    values (@familyId, (
                        select id from buildings
                        where type = @type
                    ))
                    on conflict (family_id, building_id) do nothing",
                    new {familyId, type});
    }
}
