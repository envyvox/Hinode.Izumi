using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Commands
{
    public record RemoveCertificateFromUserCommand(long UserId, long CertificateId) : IRequest;
}
