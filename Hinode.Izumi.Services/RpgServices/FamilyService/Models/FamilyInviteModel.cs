using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.FamilyService.Models
{
    /// <summary>
    /// Приглешние в семью.
    /// </summary>
    public class FamilyInviteModel : EntityBaseModel
    {
        /// <summary>
        /// Id семьи.
        /// </summary>
        public long FamilyId { get; set; }

        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }
    }
}
