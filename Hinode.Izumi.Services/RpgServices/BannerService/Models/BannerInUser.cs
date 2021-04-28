using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Services.RpgServices.BannerService.Models
{
    /// <summary>
    /// Баннер у пользователя.
    /// </summary>
    public class BannerInUser : UserBannerModel
    {
        /// <summary>
        /// Редкость баннера.
        /// </summary>
        public BannerRarity Rarity { get; set; }

        /// <summary>
        /// Название баннера.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Цена баннера.
        /// </summary>
        public long Price { get; set; }
    }
}
