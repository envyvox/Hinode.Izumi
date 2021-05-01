using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.ProjectService.Models;

namespace Hinode.Izumi.Services.RpgServices.ProjectService
{
    public interface IProjectService
    {
        /// <summary>
        /// Возвращает все чертежи.
        /// </summary>
        /// <returns>Массив чертежей.</returns>
        Task<ProjectModel[]> GetAllProjects();

        /// <summary>
        /// Возвращает массив чертежей у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив чертежей.</returns>
        Task<ProjectModel[]> GetUserProject(long userId);

        /// <summary>
        /// Возвращает чертеж. Кэшируется.
        /// </summary>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>Чертеж.</returns>
        /// <exception cref="IzumiNullableMessage.Project"></exception>
        Task<ProjectModel> GetProject(long projectId);

        /// <summary>
        /// Проверяет наличие чертежа у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id чертежа.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckUserHasProject(long userId, long projectId);

        /// <summary>
        /// Добавляет чертеж пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id чертежа.</param>
        Task AddProjectToUser(long userId, long projectId);

        /// <summary>
        /// Забирает чертеж у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="projectId">Id чертежа.</param>
        Task RemoveProjectFromUser(long userId, long projectId);
    }
}
