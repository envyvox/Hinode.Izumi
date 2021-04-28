using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.BannerService.Models
{
    /// <summary>
    /// Баннер у пользователя.
    /// </summary>
    public class UserBannerModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id баннера.
        /// </summary>
        public long BannerId { get; set; }

        /// <summary>
        /// Текущий статус баннера.
        /// </summary>
        public bool Active { get; set; }
    }
}
