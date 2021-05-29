using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Framework.Database;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Commands
{
    public record AddCertificateToUserCommand(long UserId, long CertificateId) : IRequest;

    public class AddCertificateToUserHandler : IRequestHandler<AddCertificateToUserCommand>
    {
        private readonly IConnectionManager _con;

        public AddCertificateToUserHandler(IConnectionManager con)
        {
            _con = con;
        }

        public async Task<Unit> Handle(AddCertificateToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, certificateId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_certificates(user_id, certificate_id)
                    values (@userId, @certificateId)
                    on conflict (user_id, certificate_id) do nothing",
                    new {userId, certificateId});

            return new Unit();
        }
    }
}
