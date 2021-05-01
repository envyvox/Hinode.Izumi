using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.CropService.Models;

namespace Hinode.Izumi.Services.RpgServices.CropService
{
    public interface ICropService
    {
        /// <summary>
        /// Возвращает массив урожаев.
        /// </summary>
        /// <returns>Массив урожаев.</returns>
        Task<CropModel[]> GetAllCrops();

        /// <summary>
        /// Возвращает урожай. Кэшируется.
        /// </summary>
        /// <param name="id">Id урожая.</param>
        /// <returns>Урожай.</returns>
        Task<CropModel> GetCrop(long id);

        /// <summary>
        /// Возвращает урожай. Кэшируется.
        /// </summary>
        /// <param name="seedId">Id семени.</param>
        /// <returns>Урожай.</returns>
        Task<CropModel> GetCropBySeedId(long seedId);

        /// <summary>
        /// Возвращает случайный урожай.
        /// </summary>
        /// <returns>Случайный урожай.</returns>
        Task<CropModel> GetRandomCrop();
    }
}
