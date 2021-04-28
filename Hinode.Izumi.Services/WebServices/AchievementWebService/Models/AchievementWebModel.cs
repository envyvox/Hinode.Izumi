using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.AchievementWebService.Models
{
    /// <summary>
    /// Достижение.
    /// </summary>
    public class AchievementWebModel : EntityBaseModel
    {
        /// <summary>
        /// Категория достижения.
        /// </summary>
        public AchievementCategory Category { get; set; }

        /// <summary>
        /// Достижение.
        /// </summary>
        public Achievement Type { get; set; }

        /// <summary>
        /// Тип награды за достижение.
        /// </summary>
        public AchievementReward Reward { get; set; }

        /// <summary>
        /// Количество или номер выдаваемого предмета.
        /// </summary>
        public long Number { get; set; }
    }
}
