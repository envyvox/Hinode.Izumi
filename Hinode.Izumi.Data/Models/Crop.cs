namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Урожай, который вырастает из семян.
    /// </summary>
    public class Crop : EntityBase
    {
        /// <summary>
        /// Название урожая.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена урожая.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Id семени, из которого вырастает этот урожай.
        /// </summary>
        public long SeedId { get; set; }

        /// <summary>
        /// Семя, из которого вырастает этот урожай.
        /// </summary>
        public virtual Seed Seed { get; set; }
    }
}
