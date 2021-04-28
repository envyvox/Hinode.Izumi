using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    /// <summary>
    /// Валюта семьи.
    /// </summary>
    public class FamilyCurrency : EntityBase
    {
        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Количество валюты у семьи.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Семья.
        /// </summary>
        public virtual Family Family { get; set; }
    }
}
