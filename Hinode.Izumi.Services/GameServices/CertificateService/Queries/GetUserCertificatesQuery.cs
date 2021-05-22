using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.CertificateService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Queries
{
    public record GetUserCertificatesQuery(long UserId) : IRequest<Dictionary<Certificate, CertificateRecord>>;
}
