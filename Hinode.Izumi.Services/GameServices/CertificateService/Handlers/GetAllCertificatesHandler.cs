using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CertificateService.Queries;
using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Handlers
{
    public class GetAllCertificatesHandler : IRequestHandler<GetAllCertificatesQuery, CertificateRecord[]>
    {
        private readonly IConnectionManager _con;

        public GetAllCertificatesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CertificateRecord[]> Handle(GetAllCertificatesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CertificateRecord>(@"
                        select * from certificates
                        order by type"))
                .ToArray();
        }
    }
}
