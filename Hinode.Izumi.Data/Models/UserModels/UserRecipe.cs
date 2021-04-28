namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Рецепт блюда у пользователя.
    /// </summary>
    public class UserRecipe : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id блюда.
        /// </summary>
        public long FoodId { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Блюдо.
        /// </summary>
        public virtual Food Food { get; set; }
    }
}
