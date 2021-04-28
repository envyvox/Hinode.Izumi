using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CertificateService.Models
{
    /// <summary>
    /// Сертификат.
    /// </summary>
    public class CertificateModel : EntityBaseModel
    {
        /// <summary>
        /// Сертификат.
        /// </summary>
        public Certificate Type { get; set; }

        /// <summary>
        /// Название сертификата.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание сертификата.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Цена сертификата.
        /// </summary>
        public long Price { get; set; }
    }
}
