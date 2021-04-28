using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.CardService.Models
{
    /// <summary>
    /// Карточка.
    /// </summary>
    public class CardModel : EntityBaseModel
    {
        /// <summary>
        /// Редкость карточки.
        /// </summary>
        public CardRarity Rarity { get; set; }

        /// <summary>
        /// Эффект карточки.
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// Название карточки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название тайтла, изображенного на карточке.
        /// </summary>
        public string Anime { get; set; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        public string Url { get; set; }
    }
}
