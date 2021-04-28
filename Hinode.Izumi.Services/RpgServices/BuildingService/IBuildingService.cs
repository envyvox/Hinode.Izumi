using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.BuildingService.Models;

namespace Hinode.Izumi.Services.RpgServices.BuildingService
{
    public interface IBuildingService
    {
        /// <summary>
        /// Возвращает постройку.
        /// </summary>
        /// <param name="buildingId">Id постройки.</param>
        /// <returns>Постройка.</returns>
        /// <exception cref="IzumiNullableMessage.Building"></exception>
        Task<BuildingModel> GetBuilding(long buildingId);

        /// <summary>
        /// Возвращает постройку.
        /// </summary>
        /// <param name="type">Постройка.</param>
        /// <returns>Постройка.</returns>
        /// <exception cref="IzumiNullableMessage.Building"></exception>
        Task<BuildingModel> GetBuilding(Building type);

        /// <summary>
        /// Возвращает постройку.
        /// </summary>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>Постройка.</returns>
        /// <exception cref="IzumiNullableMessage.Building"></exception>
        Task<BuildingModel> GetBuildingByProjectId(long projectId);

        /// <summary>
        /// Возвращает массив построек пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив построек пользователя.</returns>
        Task<BuildingModel[]> GetUserBuildings(long userId);

        /// <summary>
        /// Возвращает массив построек семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <returns>Массив построек семьи.</returns>
        Task<BuildingModel[]> GetFamilyBuildings(long familyId);

        /// <summary>
        /// Проверяет наличие постройки у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="type">Постройка.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckBuildingInUser(long userId, Building type);

        /// <summary>
        /// Проверяет наличие постройки у семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="type">Постройка.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckBuildingInFamily(long familyId, Building type);

        /// <summary>
        /// Добавляет постройку пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="type">Постройка..</param>
        Task AddBuildingToUser(long userId, Building type);

        /// <summary>
        /// Добавляет постройку семье.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="type">Постройка.</param>
        Task AddBuildingToFamily(long familyId, Building type);
    }
}
