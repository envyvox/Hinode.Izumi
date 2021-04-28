using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.UserService.Models
{
    /// <summary>
    /// Титул пользователя.
    /// </summary>
    public class UserTitleModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Титул.
        /// </summary>
        public Title Title { get; set; }
    }
}
