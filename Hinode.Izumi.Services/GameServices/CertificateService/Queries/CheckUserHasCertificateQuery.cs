using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record CheckUserHasCertificateQuery(long UserId, long CertificateId) : IRequest<bool>;
}
