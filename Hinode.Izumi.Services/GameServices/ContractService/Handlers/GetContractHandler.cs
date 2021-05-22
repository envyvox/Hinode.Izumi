using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.ContractService.Queries;
using Hinode.Izumi.Services.GameServices.ContractService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.ContractService.Handlers
{
    public class GetContractHandler : IRequestHandler<GetContractQuery, ContractRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetContractHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<ContractRecord> Handle(GetContractQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.ContractKey, request.Id), out ContractRecord contract))
                return contract;

            contract = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<ContractRecord>(@"
                    select * from contracts
                    where id = @id",
                    new {id = request.Id});

            if (contract is not null)
            {
                _cache.Set(string.Format(CacheExtensions.ContractKey, request.Id), contract,
                    CacheExtensions.DefaultCacheOptions);
                return contract;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.ContractById.Parse()));
            return null;
        }
    }
}
