using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Постройка.
    /// </summary>
    public class Building : EntityBase
    {
        /// <summary>
        /// Постройка.
        /// </summary>
        public Enums.Building Type { get; set; }

        /// <summary>
        /// Id чертежа (не обязательно).
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// Категория постройки.
        /// </summary>
        public BuildingCategory Category { get; set; }

        /// <summary>
        /// Название постройки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание постройки.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Чертеж.
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
