using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetUserCertificateQuery(long UserId, long CertificateId) : IRequest<CertificateRecord>;
}
