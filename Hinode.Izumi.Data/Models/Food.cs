namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Блюдо.
    /// </summary>
    public class Food : EntityBase
    {
        /// <summary>
        /// Название блюда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Необходимое мастерство для приготовления этого блюда.
        /// </summary>
        public long Mastery { get; set; }

        /// <summary>
        /// Длительность приготовления одной единицы этого блюда.
        /// </summary>
        public long Time { get; set; }
    }
}
