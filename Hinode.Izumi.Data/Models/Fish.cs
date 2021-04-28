using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Рыба.
    /// </summary>
    public class Fish : EntityBase
    {
        /// <summary>
        /// Название рыбы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Редкость рыбы.
        /// </summary>
        public FishRarity Rarity { get; set; }

        /// <summary>
        /// Сезоны, в которые ее можно поймать и продать.
        /// </summary>
        public Season[] Seasons { get; set; }

        /// <summary>
        /// Погода, в которую ее можно поймать.
        /// </summary>
        public Weather Weather { get; set; }

        /// <summary>
        /// Время суток, в которое ее можно поймать.
        /// </summary>
        public TimesDay TimesDay { get; set; }

        /// <summary>
        /// Цена рыбы.
        /// </summary>
        public long Price { get; set; }
    }
}
