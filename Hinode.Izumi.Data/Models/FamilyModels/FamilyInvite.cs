using Hinode.Izumi.Data.Models.UserModels;

namespace Hinode.Izumi.Data.Models.FamilyModels
{
    /// <summary>
    /// Приглешние в семью.
    /// </summary>
    public class FamilyInvite : EntityBase
    {
        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Семья.
        /// </summary>
        public virtual Family Family { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }
    }
}
