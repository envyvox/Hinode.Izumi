namespace Hinode.Izumi.Data.Models.UserModels
{
    public class UserCertificate : EntityBase
    {
        public long UserId { get; set; }
        public long CertificateId { get; set; }
        public virtual User User { get; set; }
        public virtual Certificate Certificate { get; set; }
    }
}
