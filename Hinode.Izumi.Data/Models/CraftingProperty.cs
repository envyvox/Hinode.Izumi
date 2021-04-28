namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Свойство изготавливаемого предмета.
    /// </summary>
    public class CraftingProperty : EntityBase
    {
        /// <summary>
        /// Id изготавливаемого предмета.
        /// </summary>
        public long CraftingId { get; set; }

        /// <summary>
        /// Свойство изготавливаемого предмета.
        /// </summary>
        public Enums.PropertyEnums.CraftingProperty Property { get; set; }

        /// <summary>
        /// Значение при 0 мастерства.
        /// </summary>
        public long Mastery0 { get; set; }

        /// <summary>
        /// Значение при 50 мастерства.
        /// </summary>
        public long Mastery50 { get; set; }

        /// <summary>
        /// Значение при 100 мастерства.
        /// </summary>
        public long Mastery100 { get; set; }

        /// <summary>
        /// Значение при 150 мастерства.
        /// </summary>
        public long Mastery150 { get; set; }

        /// <summary>
        /// Значение при 200 мастерства.
        /// </summary>
        public long Mastery200 { get; set; }

        /// <summary>
        /// Значение при 250 мастерства.
        /// </summary>
        public long Mastery250 { get; set; }

        /// <summary>
        /// Изготовленный предмет.
        /// </summary>
        public virtual Crafting Crafting { get; set; }
    }
}
