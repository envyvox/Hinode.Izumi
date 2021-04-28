using Hinode.Izumi.Data.Enums.AchievementEnums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Достижение.
    /// </summary>
    public class Achievement : EntityBase
    {
        /// <summary>
        /// Категория достижения.
        /// </summary>
        public AchievementCategory Category { get; set; }

        /// <summary>
        /// Достижение.
        /// </summary>
        public Enums.AchievementEnums.Achievement Type { get; set; }

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
