using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.PropertyService.Models
{
    /// <summary>
    /// Свойство опыта мастерства.
    /// </summary>
    public class MasteryXpPropertyModel : EntityBaseModel
    {
        /// <summary>
        /// Свойство опыта мастерства.
        /// </summary>
        public MasteryXpProperty Property { get; set; }

        /// <summary>
        /// Значение при 0 мастерства.
        /// </summary>
        public double Mastery0 { get; set; }

        /// <summary>
        /// Значение при 50 мастерства.
        /// </summary>
        public double Mastery50 { get; set; }

        /// <summary>
        /// Значение при 100 мастерства.
        /// </summary>
        public double Mastery100 { get; set; }

        /// <summary>
        /// Значение при 150 мастерства.
        /// </summary>
        public double Mastery150 { get; set; }

        /// <summary>
        /// Значение при 200 мастерства.
        /// </summary>
        public double Mastery200 { get; set; }

        /// <summary>
        /// Значение при 250 мастерства.
        /// </summary>
        public double Mastery250 { get; set; }
    }
}
