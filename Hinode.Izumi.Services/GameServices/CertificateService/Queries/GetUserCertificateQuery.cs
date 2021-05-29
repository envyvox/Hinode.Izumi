using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetUserCertificateQuery(long UserId, long CertificateId) : IRequest<CertificateRecord>;

    public class GetUserCertificateHandler : IRequestHandler<GetUserCertificateQuery, CertificateRecord>
    {
        private readonly IConnectionManager _con;

        public GetUserCertificateHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<CertificateRecord> Handle(GetUserCertificateQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, certificateId) = request;
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CertificateRecord>(@"
                    select c.* from user_certificates as uc
                        inner join certificates c
                            on c.id = uc.certificate_id
                    where uc.user_id = @userId
                      and uc.certificate_id = @certificateId",
                    new {userId, certificateId});

            if (res is null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserCertificate.Parse()));

            return res;
        }
    }
}
