using Hinode.Izumi.Data.Enums.PropertyEnums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Свойства (настройки) игрового мира.
    /// </summary>
    public class WorldProperty : EntityBase
    {
        /// <summary>
        /// Категория свойства игрового мира.
        /// </summary>
        public PropertyCategory PropertyCategory { get; set; }

        /// <summary>
        /// Свойство игрового мира.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public long Value { get; set; }
    }
}
