namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Изготавливаемые предметы у пользователя.
    /// </summary>
    public class UserCrafting : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id изготавливаемого предмета.
        /// </summary>
        public long CraftingId { get; set; }

        /// <summary>
        /// Количество изготавливаемого предмета у пользователя.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Изготавливаемый предмет.
        /// </summary>
        public virtual Crafting Crafting { get; set; }
    }
}
