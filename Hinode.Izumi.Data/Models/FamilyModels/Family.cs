using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    /// <summary>
    /// Семья.
    /// </summary>
    public class Family : EntityBase
    {
        /// <summary>
        /// Статус семьи.
        /// </summary>
        public FamilyStatus Status { get; set; }

        /// <summary>
        /// Название семьи.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание семьи.
        /// </summary>
        public string Description { get; set; }
    }
}
