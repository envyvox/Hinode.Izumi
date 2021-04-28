namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Свойство (настройка) собирательского ресурса.
    /// </summary>
    public class GatheringProperty : EntityBase
    {
        /// <summary>
        /// Id собирательского ресурса.
        /// </summary>
        public long GatheringId { get; set; }

        /// <summary>
        /// Свойство собирательского ресурса.
        /// </summary>
        public Enums.PropertyEnums.GatheringProperty Property { get; set; }

        /// <summary>
        /// Значение при 0 мастерства.
        /// </summary>
        public long Mastery0 { get; set; }

        /// <summary>
        /// Значение при 50 мастерства.
        /// </summary>
        public long Mastery50 { get; set; }

        /// <summary>
        /// Значение при 100 мастерства.
        /// </summary>
        public long Mastery100 { get; set; }

        /// <summary>
        /// Значение при 150 мастерства.
        /// </summary>
        public long Mastery150 { get; set; }

        /// <summary>
        /// Значение при 200 мастерства.
        /// </summary>
        public long Mastery200 { get; set; }

        /// <summary>
        /// Значение при 250 мастерства.
        /// </summary>
        public long Mastery250 { get; set; }

        /// <summary>
        /// Собирательский ресурс.
        /// </summary>
        public virtual Gathering Gathering { get; set; }
    }
}
