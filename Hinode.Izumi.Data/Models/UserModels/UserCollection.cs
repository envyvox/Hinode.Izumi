using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Коллекция пользователя.
    /// </summary>
    public class UserCollection : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Категория коллекции.
        /// </summary>
        public CollectionCategory Category { get; set; }

        /// <summary>
        /// Id предмета.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Событие на котором получается этот предмет.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
