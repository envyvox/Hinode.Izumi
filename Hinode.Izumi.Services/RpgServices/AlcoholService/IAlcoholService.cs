using System.Threading.Tasks;
using Hinode.Izumi.Services.RpgServices.AlcoholService.Models;

namespace Hinode.Izumi.Services.RpgServices.AlcoholService
{
    public interface IAlcoholService
    {
        /// <summary>
        /// Возвращает массив алкоголя.
        /// </summary>
        /// <returns>Массив алкоголя.</returns>
        Task<AlcoholModel[]> GetAllAlcohol();

        /// <summary>
        /// Возвращает алкоголь. Кэшируется.
        /// </summary>
        /// <param name="id">Id алкоголя.</param>
        /// <returns>Алкоголь.</returns>
        Task<AlcoholModel> GetAlcohol(long id);
    }
}
