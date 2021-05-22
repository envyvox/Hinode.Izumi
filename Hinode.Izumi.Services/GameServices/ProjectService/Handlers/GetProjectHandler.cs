using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.GameServices.ProjectService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.ProjectService.Handlers
{
    public class GetProjectHandler : IRequestHandler<GetProjectQuery, ProjectRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetProjectHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ProjectRecord> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.ProjectKey, request.Id), out ProjectRecord project))
                return project;

            project = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ProjectRecord>(@"
                    select * from projects
                    where id = @id",
                    new {id = request.Id});

            if (project is not null)
            {
                _cache.Set(string.Format(CacheExtensions.ProjectKey, request.Id), project,
                    CacheExtensions.DefaultCacheOptions);
                return project;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Project.Parse()));
            return null;
        }
    }
}
