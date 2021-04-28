using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.CardService.Models;

namespace Hinode.Izumi.Services.RpgServices.CardService
{
    public interface ICardService
    {
        /// <summary>
        /// Возвращает карточку. Кэшируется.
        /// </summary>
        /// <param name="id">Id карточки.</param>
        /// <returns>Карточка.</returns>
        /// <exception cref="IzumiNullableMessage.CardById"></exception>
        Task<CardModel> GetCard(long id);

        /// <summary>
        /// Возвращает карточку у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cardId">Id карточки.</param>
        /// <returns>Карточка у пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserCard"></exception>
        Task<CardModel> GetUserCard(long userId, long cardId);

        /// <summary>
        /// Возвращает массив карточек пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив карточек пользователя.</returns>
        Task<CardModel[]> GetUserCard(long userId);

        /// <summary>
        /// Возвращает массив карточек в колоде пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив карточек в колоде пользователя.</returns>
        Task<CardModel[]> GetUserDeck(long userId);

        /// <summary>
        /// Проверяет есть ли карточка в колоде пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cardId">Id карточки.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckCardInUserDeck(long userId, long cardId);

        /// <summary>
        /// Возвращает количество карточек в колоде пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Количество карточек в колоде пользователя.</returns>
        Task<long> GetUserDeckLength(long userId);

        /// <summary>
        /// Возвращает общее количество карточек в игровом мире.
        /// </summary>
        /// <returns>Количество карточек в игровом мире.</returns>
        Task<long> GetAllCardLength();

        /// <summary>
        /// Добавляет карточку пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cardId">Id карточки.</param>
        Task AddCardToUser(long userId, long cardId);

        /// <summary>
        /// Добавляет карточку в колоду пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cardId">Id карточки.</param>
        Task AddCardToDeck(long userId, long cardId);

        /// <summary>
        /// Убирает карточку с колоды пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="cardId">Id карточки.</param>
        Task RemoveCardFromDeck(long userId, long cardId);
    }
}
