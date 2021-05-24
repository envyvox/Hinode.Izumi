using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetCertificateQuery(long Id) : IRequest<CertificateRecord>;

    public class GetCertificateHandler : IRequestHandler<GetCertificateQuery, CertificateRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public GetCertificateHandler(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CertificateRecord> Handle(GetCertificateQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(string.Format(CacheExtensions.CertificateKey, request.Id),
                out CertificateRecord certificate)) return certificate;

            certificate = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CertificateRecord>(@"
                    select * from certificates
                    where id = @certificateId",
                    new {certificateId = request.Id});

            if (certificate is not null)
            {
                _cache.Set(string.Format(CacheExtensions.CertificateKey, request.Id), certificate,
                    CacheExtensions.DefaultCacheOptions);
                return certificate;
            }

            await Task.FromException(new Exception(IzumiNullableMessage.Certificate.Parse()));
            return null;
        }
    }
}
