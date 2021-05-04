using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : EntityBase
    {
        /// <summary>
        /// Игровое имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Информация о пользователе отображаемая в профиле.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Текущий титул пользователя.
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// Пол пользователя.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Текущая локация пользователя.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Количество энергии у пользователя.
        /// </summary>
        public int Energy { get; set; }

        /// <summary>
        /// Количество очков приключений.
        /// </summary>
        public long Points { get; set; }
    }
}
