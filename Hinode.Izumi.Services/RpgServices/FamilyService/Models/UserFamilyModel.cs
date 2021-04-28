using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FamilyService.Models
{
    /// <summary>
    /// Семья пользователя.
    /// </summary>
    public class UserFamilyModel : EntityBaseModel
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
    }
}
