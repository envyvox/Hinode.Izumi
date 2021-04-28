using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.GatheringService.Models;

namespace Hinode.Izumi.Services.RpgServices.GatheringService
{
    public interface IGatheringService
    {
        /// <summary>
        /// Возвращает массив собирательских ресурсов.
        /// </summary>
        /// <returns>Массив собирательских ресурсов.</returns>
        Task<GatheringModel[]> GetAllGatherings();

        /// <summary>
        /// Возвращает массив собирательских ресурсов доступных в указанной локации.
        /// </summary>
        /// <param name="location">Локация.</param>
        /// <returns>Массив собирательских ресурсов доступных в указанной локации.</returns>
        Task<GatheringModel[]> GetGathering(Location location);

        /// <summary>
        /// Возвращает собирательский ресурс. Кэшируется.
        /// </summary>
        /// <param name="id">Id собирательского ресурса.</param>
        /// <returns>Собирательский ресурс.</returns>
        Task<GatheringModel> GetGathering(long id);
    }
}
