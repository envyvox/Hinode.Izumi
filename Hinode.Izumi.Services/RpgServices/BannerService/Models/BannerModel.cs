using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.BannerService.Models
{
    /// <summary>
    /// Баннер в профиле.
    /// </summary>
    public class BannerModel : EntityBaseModel
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
        /// Название тайтла.
        /// </summary>
        public string Anime { get; set; }

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
