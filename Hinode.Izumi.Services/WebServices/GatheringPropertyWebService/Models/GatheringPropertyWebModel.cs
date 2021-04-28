using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.GatheringPropertyWebService.Models
{
    /// <summary>
    /// Свойство (настройка) собирательского ресурса.
    /// </summary>
    public class GatheringPropertyWebModel : EntityBaseModel
    {
        /// <summary>
        /// Id собирательского ресурса.
        /// </summary>
        public long GatheringId { get; set; }

        /// <summary>
        /// Название собирательского ресурса.
        /// </summary>
        public string GatheringName { get; set; }

        /// <summary>
        /// Свойство собирательского ресурса.
        /// </summary>
        public GatheringProperty Property { get; set; }

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
    }
}
