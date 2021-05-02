using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.UserService.Models
{
    public class UserModel : EntityBaseModel
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
