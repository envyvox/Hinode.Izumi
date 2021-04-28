namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Карточка в колоде пользователя.
    /// </summary>
    public class UserDeck : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id карточки.
        /// </summary>
        public long CardId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Карточка.
        /// </summary>
        public virtual Card Card { get; set; }
    }
}
