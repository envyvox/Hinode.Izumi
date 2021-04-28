using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.SeedService.Models;

namespace Hinode.Izumi.Services.RpgServices.SeedService
{
    public interface ISeedService
    {
        /// <summary>
        /// Возвращает массив семян указанного сезона.
        /// </summary>
        /// <param name="season">Сезон.</param>
        /// <returns>Массив семян указанного сезона.</returns>
        Task<SeedModel[]> GetSeed(Season season);

        /// <summary>
        /// Возвращает семя. Кэшируется.
        /// </summary>
        /// <param name="id">Id семени.</param>
        /// <returns>Семя.</returns>
        Task<SeedModel> GetSeed(long id);

        /// <summary>
        /// Возвращает семя. Кэшируется.
        /// </summary>
        /// <param name="namePattern">Название семени.</param>
        /// <returns>Семя.</returns>
        /// <exception cref="IzumiNullableMessage"></exception>
        Task<SeedModel> GetSeed(string namePattern);
    }
}
