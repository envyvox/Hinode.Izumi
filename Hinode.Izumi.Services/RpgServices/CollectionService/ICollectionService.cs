using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.CollectionService.Models;

namespace Hinode.Izumi.Services.RpgServices.CollectionService
{
    public interface ICollectionService
    {
        /// <summary>
        /// Возвращает массив статистики пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория статистики.</param>
        /// <returns>Массив статистики пользователя.</returns>
        Task<UserCollectionModel[]> GetUserCollection(long userId, CollectionCategory category);

        /// <summary>
        /// Добавляет статистику пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория статистики.</param>
        /// <param name="itemId">Id предмета.</param>
        Task AddCollectionToUser(long userId, CollectionCategory category, long itemId);
    }
}
