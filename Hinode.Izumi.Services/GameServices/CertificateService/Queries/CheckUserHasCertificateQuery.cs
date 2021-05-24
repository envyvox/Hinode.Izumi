using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record CheckUserHasCertificateQuery(long UserId, long CertificateId) : IRequest<bool>;

    public class CheckUserHasCertificateHandler : IRequestHandler<CheckUserHasCertificateQuery, bool>
    {
        private readonly IConnectionManager _con;

        public CheckUserHasCertificateHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<bool> Handle(CheckUserHasCertificateQuery request, CancellationToken cancellationToken)
        {
            var (userId, certificateId) = request;
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_certificates
                    where user_id = @userId
                      and certificate_id = @certificateId",
                    new {userId, certificateId});
        }
    }
}
