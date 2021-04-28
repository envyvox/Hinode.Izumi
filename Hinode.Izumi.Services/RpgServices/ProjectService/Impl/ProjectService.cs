using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.ProjectService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.ProjectService.Impl
{
    [InjectableService]
    public class ProjectService : IProjectService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public ProjectService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ProjectModel[]> GetAllProjects() =>
            (await _con.GetConnection()
                .QueryAsync<ProjectModel>(@"
                    select * from projects
                    order by id"))
            .ToArray();

        public async Task<ProjectModel[]> GetUserProject(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<ProjectModel>(@"
                    select p.* from user_projects as up
                        inner join projects p
                            on p.id = up.project_id
                    where up.user_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<ProjectModel> GetProject(long projectId)
        {
            // проверяем чертеж в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.ProjectKey, projectId), out ProjectModel project))
                return project;

            // получаем чертеж из базы
            project = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ProjectModel>(@"
                    select * from projects
                    where id = @projectId",
                    new {projectId});

            // если такой чертеж есть
            if (project != null)
            {
                // добавляем его в кэш
                _cache.Set(string.Format(CacheExtensions.ProjectKey, projectId), project,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return project;
            }

            // если нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Project.Parse()));
            return new ProjectModel();
        }

        public async Task<bool> CheckUserHasProject(long userId, long projectId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_projects
                    where user_id = @userId
                      and project_id = @projectId",
                    new {userId, projectId});

        public async Task AddProjectToUser(long userId, long projectId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_projects(user_id, project_id)
                    values (@userId, @projectId)
                    on conflict (user_id, project_id) do nothing",
                    new {userId, projectId});

        public async Task RemoveProjectFromUser(long userId, long projectId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_projects
                    where user_id = @userId
                      and project_id = @projectId",
                    new {userId, projectId});
    }
}
