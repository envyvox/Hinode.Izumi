using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.BuildingService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.BuildingService.Queries
{
    public record GetBuildingByIdQuery(long Id) : IRequest<BuildingRecord>;

    public class GetBuildingByIdHandler : IRequestHandler<GetBuildingByIdQuery, BuildingRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetBuildingByIdHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<BuildingRecord> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.BuildingIdKey, request.Id),
                out BuildingRecord building)) return building;

            building = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<BuildingRecord>(@"
                    select * from buildings
                    where id = @buildingId",
                    new {buildingId = request.Id});

            if (building is not null)
            {
                _cache.Set(string.Format(CacheExtensions.BuildingIdKey, request.Id), building,
                    CacheExtensions.DefaultCacheOptions);
                return building;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Building.Parse()));
            return null;
        }
    }
}
