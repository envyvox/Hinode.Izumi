using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Семя для выращивания урожая на участке.
    /// </summary>
    public class Seed : EntityBase
    {
        /// <summary>
        /// Название семени.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сезон, в котором его можно купить и посадить.
        /// </summary>
        public Season Season { get; set; }

        /// <summary>
        /// Количество дней роста.
        /// </summary>
        public long Growth { get; set; }

        /// <summary>
        /// Количество дней повторного роста (установить на 0, если семя не растет повторно).
        /// </summary>
        public long ReGrowth { get; set; }

        /// <summary>
        /// Цена семени.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Вырастает ли несколько единиц урожая с одного семени.
        /// </summary>
        public bool Multiply { get; set; }
    }
}
