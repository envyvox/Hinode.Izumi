using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.CertificateService.Records
{
    public record CertificateRecord(long Id, Certificate Type, string Name, string Description, long Price)
    {
        public CertificateRecord() : this(default, default, default, default, default)
        {
        }
    }
}
