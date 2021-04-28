using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ReputationService.Models
{
    /// <summary>
    /// Репутация у пользователя.
    /// </summary>
    public class UserReputationModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Репутация.
        /// </summary>
        public Reputation Reputation { get; set; }

        /// <summary>
        /// Количество репутации у пользователя.
        /// </summary>
        public long Amount { get; set; }
    }
}
