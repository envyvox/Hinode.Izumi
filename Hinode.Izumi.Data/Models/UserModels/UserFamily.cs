using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Models.FamilyModels;

namespace Hinode.Izumi.Data.Models.UserModels
{
    /// <summary>
    /// Семья пользователя.
    /// </summary>
    public class UserFamily : EntityBase
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Статус пользователя в семье.
        /// </summary>
        public UserInFamilyStatus Status { get; set; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Семья.
        /// </summary>
        public virtual Family Family { get; set; }
    }
}
