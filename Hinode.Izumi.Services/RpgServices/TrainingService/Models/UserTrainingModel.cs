using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.TrainingService.Models
{
    /// <summary>
    /// Шаг обучения у пользователя.
    /// </summary>
    public class UserTrainingModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Шаг обучения.
        /// </summary>
        public TrainingStep Step { get; set; }
    }
}
