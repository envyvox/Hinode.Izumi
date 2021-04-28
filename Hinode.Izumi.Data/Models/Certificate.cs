namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Сертификат.
    /// </summary>
    public class Certificate : EntityBase
    {
        /// <summary>
        /// Сертификат.
        /// </summary>
        public Enums.Certificate Type { get; set; }

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
