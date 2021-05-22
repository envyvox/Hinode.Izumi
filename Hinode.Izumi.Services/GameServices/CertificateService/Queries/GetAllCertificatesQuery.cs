using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetAllCertificatesQuery : IRequest<CertificateRecord[]>;
}
