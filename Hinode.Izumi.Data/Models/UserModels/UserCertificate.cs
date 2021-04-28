namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Сертификат у пользователя.
    /// </summary>
    public class UserCertificate : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id сертификата.
        /// </summary>
        public long CertificateId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Сертификат.
        /// </summary>
        public virtual Certificate Certificate { get; set; }
    }
}
