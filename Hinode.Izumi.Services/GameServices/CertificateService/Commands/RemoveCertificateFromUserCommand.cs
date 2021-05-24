using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Commands
{
    public record RemoveCertificateFromUserCommand(long UserId, long CertificateId) : IRequest;

    public class RemoveCertificateFromUserHandler : IRequestHandler<RemoveCertificateFromUserCommand>
    {
        private readonly IConnectionManager _con;

        public RemoveCertificateFromUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(RemoveCertificateFromUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, certificateId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_certificates
                    where user_id = @userId
                      and certificate_id = @certificateId",
                    new {userId, certificateId});

            return new Unit();
        }
    }
}
