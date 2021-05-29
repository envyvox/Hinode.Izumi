using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ContractService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.ContractService.Queries
{
    public record GetContractsInLocationQuery(Location Location) : IRequest<ContractRecord[]>;

    public class GetContractsInLocationHandler : IRequestHandler<GetContractsInLocationQuery, ContractRecord[]>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetContractsInLocationHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ContractRecord[]> Handle(GetContractsInLocationQuery request,
            CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.ContractLocationKey, request.Location),
                out ContractRecord[] contracts)) return contracts;

            contracts = (await _con.GetConnection()
                    .QueryAsync<ContractRecord>(@"
                        select * from contracts
                        where location = @location",
                        new {location = request.Location}))
                .ToArray();

            if (contracts.Length > 0)
            {
                _cache.Set(string.Format(CacheExtensions.ContractLocationKey, request.Location), contracts,
                    CacheExtensions.DefaultCacheOptions);
                return contracts;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.ContractByLocation.Parse()));
            return Array.Empty<ContractRecord>();
        }
    }
}
