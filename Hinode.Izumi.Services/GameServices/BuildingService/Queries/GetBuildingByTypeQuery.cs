using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record GetBuildingByTypeQuery(Building Type) : IRequest<BuildingRecord>;

    public class GetBuildingByTypeHandler : IRequestHandler<GetBuildingByTypeQuery, BuildingRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetBuildingByTypeHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<BuildingRecord> Handle(GetBuildingByTypeQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.BuildingTypeKey, request.Type),
                out BuildingRecord building)) return building;

            building = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BuildingRecord>(@"
                    select * from buildings
                    where type = @type",
                    new {type = request.Type});

            if (building is not null)
            {
                _cache.Set(string.Format(CacheExtensions.BuildingTypeKey, request.Type), building,
                    CacheExtensions.DefaultCacheOptions);
                return building;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Building.Parse()));
            return null;
        }
    }
}
