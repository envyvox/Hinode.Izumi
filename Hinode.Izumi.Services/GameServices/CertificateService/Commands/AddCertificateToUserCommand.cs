using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Commands
{
    public record AddCertificateToUserCommand(long UserId, long CertificateId) : IRequest;
}
