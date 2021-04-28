using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.MarketService.Models;

namespace Hinode.Izumi.Services.RpgServices.MarketService
{
    public interface IMarketService
    {
        /// <summary>
        /// Возвращает заявку на рынке.
        /// </summary>
        /// <param name="requestId">Id заявки.</param>
        /// <returns>Заявка на рынке.</returns>
        Task<MarketRequestModel> GetMarketRequest(long requestId);

        /// <summary>
        /// Возвращает 5 заявок на рынке в указанной категории.
        /// </summary>
        /// <param name="category">Категория товара.</param>
        /// <param name="selling">Выставлен на продажу?</param>
        /// <param name="namePattern">Название товара (не обязательно).</param>
        /// <returns>Массив заявок на рынке.</returns>
        /// <exception cref="IzumiNullableMessage.MarketRequest"></exception>
        Task<MarketRequestModel[]> GetMarketRequest(MarketCategory category, bool selling, string namePattern = null);

        /// <summary>
        /// Возвращает заявку пользователя на рынке.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id товара.</param>
        /// <returns>Заявка на рынке.</returns>
        Task<MarketRequestModel> GetMarketUserRequest(long userId, MarketCategory category, long itemId);

        /// <summary>
        /// Возвращает массив заявок пользователя на рынке в указанной категории.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <returns>Массив заявок на рынке.</returns>
        Task<MarketRequestModel[]> GetMarketUserRequest(long userId, MarketCategory category);

        /// <summary>
        /// Возвращает массив заявок пользователя на рынке.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив заявок на рынке.</returns>
        Task<MarketRequestModel[]> GetMarketUserRequest(long userId);

        /// <summary>
        /// Проверяет наличие заявки пользователя на рынке.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckMarketUserRequest(long userId, MarketCategory category, long itemId);

        /// <summary>
        /// Добавляет или обновляет заявку на рынке.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id товара.</param>
        /// <param name="price">Цена товара.</param>
        /// <param name="amount">Количество товара.</param>
        /// <param name="selling">Выставлен на продажу?</param>
        Task AddOrUpdateMarketRequest(long userId, MarketCategory category, long itemId, long price, long amount,
            bool selling);

        /// <summary>
        /// Обновляет или удаляет заявку на рынке.
        /// </summary>
        /// <param name="category">Категория товара.</param>
        /// <param name="itemId">Id товара.</param>
        /// <param name="marketAmount"></param>
        /// <param name="amount">Количество товара.</param>
        Task UpdateOrDeleteMarketRequest(MarketCategory category, long itemId, long marketAmount, long amount);

        /// <summary>
        /// Удаляет заявку на рынке.
        /// </summary>
        /// <param name="requestId">Id заявки.</param>
        Task DeleteMarketRequest(long requestId);
    }
}
