using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetUserCertificatesQuery(long UserId) : IRequest<Dictionary<Certificate, CertificateRecord>>;

    public class GetUserCertificatesHandler
        : IRequestHandler<GetUserCertificatesQuery, Dictionary<Certificate, CertificateRecord>>
    {
        private readonly IConnectionManager _con;

        public GetUserCertificatesHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Dictionary<Certificate, CertificateRecord>> Handle(GetUserCertificatesQuery request,
            CancellationToken cancellationToken)
        {
            return (await _con.GetConnection()
                    .QueryAsync<CertificateRecord>(@"
                        select c.* from user_certificates as uc
                            inner join certificates c on
                                c.id = uc.certificate_id
                        where uc.user_id = @userId
                        order by type",
                        new {userId = request.UserId}))
                .ToDictionary(x => x.Type);
        }
    }
}
