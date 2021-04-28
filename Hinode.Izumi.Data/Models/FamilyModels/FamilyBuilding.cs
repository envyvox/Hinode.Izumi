namespace Hinode.Izumi.Data.Models.FamilyModels
{
    /// <summary>
    /// Постройка у семьи.
    /// </summary>
    public class FamilyBuilding : EntityBase
    {
        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Id постройки.
        /// </summary>
        public long BuildingId { get; set; }

        /// <summary>
        /// Семья.
        /// </summary>
        public virtual Family Family { get; set; }

        /// <summary>
        /// Постройка.
        /// </summary>
        public virtual Building Building { get; set; }
    }
}
