using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.RpgServices.FishService.Models;

namespace Hinode.Izumi.Services.RpgServices.FishService
{
    public interface IFishService
    {
        /// <summary>
        /// Возвращает массив рыбы.
        /// </summary>
        /// <returns>Массив рыбы.</returns>
        Task<FishModel[]> GetAllFish();

        /// <summary>
        /// Возвращает массив рыбы указанного сезона.
        /// </summary>
        /// <param name="season">Сезон.</param>
        /// <returns>Массив рыбы.</returns>
        Task<FishModel[]> GetAllFish(Season season);

        /// <summary>
        /// Возвращает рыбу. Кэшируется.
        /// </summary>
        /// <param name="id">Id рыбы.</param>
        /// <returns>Рыба.</returns>
        Task<FishModel> GetFish(long id);

        /// <summary>
        /// Возвращает случайную рыбу подходяющую по указанным параметрам.
        /// </summary>
        /// <param name="timesDay">Время суток.</param>
        /// <param name="season">Сезон.</param>
        /// <param name="weather">Погода.</param>
        /// <param name="rarity">Редкость рыбы.</param>
        /// <returns>Случайная рыба.</returns>
        Task<FishModel> GetRandomFish(TimesDay timesDay, Season season, Weather weather, FishRarity rarity);

        /// <summary>
        /// Возвращает случайную рыбу подходяющую по указанной редкости.
        /// </summary>
        /// <param name="rarity">Редкость рыбы.</param>
        /// <returns>Случайная рыба.</returns>
        Task<FishModel> GetRandomFish(FishRarity rarity);
    }
}
