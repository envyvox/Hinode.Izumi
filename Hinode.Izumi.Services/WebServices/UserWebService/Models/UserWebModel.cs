using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.WebServices.UserWebService.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class UserWebModel
    {
        /// <summary>
        /// Id пользователя.
        /// Хранится в строке потому что
        /// https://stackoverflow.com/questions/1379934/large-numbers-erroneously-rounded-in-javascript.
        /// </summary>
        public string Id { get; set; }

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
        /// Время создания записи.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Время последнего обновления записи.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
