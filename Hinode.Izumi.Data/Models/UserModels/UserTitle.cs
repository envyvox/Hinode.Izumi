using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Титул пользователя.
    /// </summary>
    public class UserTitle : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Титул.
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
