using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Баннер в профиле.
    /// </summary>
    public class Banner : EntityBase
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
