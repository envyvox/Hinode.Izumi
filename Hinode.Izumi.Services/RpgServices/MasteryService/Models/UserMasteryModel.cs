using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.MasteryService.Models
{
    /// <summary>
    /// Мастерство пользователя.
    /// </summary>
    public class UserMasteryModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Мастерство.
        /// </summary>
        public Mastery Mastery { get; set; }

        /// <summary>
        /// Количество мастерства у пользователя.
        /// </summary>
        public double Amount { get; set; }
    }
}
