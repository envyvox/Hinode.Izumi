using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.LocalizationService.Models;

namespace Hinode.Izumi.Services.RpgServices.LocalizationService
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Возвращает локализацию по категории локализации и паттерну названия.
        /// </summary>
        /// <param name="category">Категория локализации.</param>
        /// <param name="localizedWord">Локализированное название (подходит любое склонение, 1/2/5).</param>
        /// <returns>Локализация.</returns>
        /// <exception cref="IzumiNullableMessage"></exception>
        Task<LocalizationModel> GetLocalizationByLocalizedWord(LocalizationCategory category, string localizedWord);

        /// <summary>
        /// Возвращает локализацию по категории инвентаря и паттерну названия.
        /// </summary>
        /// <param name="category">Категория локализации.</param>
        /// <param name="localizedWord">Локализированное название (подходит любое склонение, 1/2/5).</param>
        /// <returns>Локализация.</returns>
        /// <exception cref="IzumiNullableMessage"></exception>
        Task<LocalizationModel> GetLocalizationByLocalizedWord(InventoryCategory category, string localizedWord);

        /// <summary>
        /// Возвращает локализацию по категории рынка и паттерну названия.
        /// </summary>
        /// <param name="category">Категория локализации.</param>
        /// <param name="localizedWord">Локализированное название (подходит любое склонение, 1/2/5).</param>
        /// <returns>Локализация.</returns>
        /// <exception cref="IzumiNullableMessage"></exception>
        Task<LocalizationModel> GetLocalizationByLocalizedWord(MarketCategory category, string localizedWord);

        /// <summary>
        /// Локазирирует название в зависимости от количества (используя приватный метод GetLocalizationByKeyword).
        /// </summary>
        /// <param name="keyword">Ключевое слово.</param>
        /// <param name="amount">Количество.</param>
        /// <returns>Локализированное название.</returns>
        /// <exception cref="IzumiNullableMessage.LocalizationByKeyword"></exception>
        string Localize(string keyword, long amount = 1);

        /// <summary>
        /// Локазирирует название в зависимости от количества (используя приватный метод GetLocalizationByKeyword).
        /// </summary>
        /// <param name="category">Категория локализации.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество.</param>
        /// <returns>Локализированное название.</returns>
        /// <exception cref="IzumiNullableMessage.LocalizationByKeyword"></exception>
        string Localize(LocalizationCategory category, long itemId, long amount = 1);

        /// <summary>
        /// Локазирирует название в зависимости от количества (используя приватный метод GetLocalizationByKeyword).
        /// </summary>
        /// <param name="requestCategory">Категория рынка.</param>
        /// <param name="itemId">Id предмета.</param>
        /// <param name="amount">Количество.</param>
        /// <returns>Локализированное название.</returns>
        /// <exception cref="IzumiNullableMessage.LocalizationByKeyword"></exception>
        string Localize(MarketCategory requestCategory, long itemId, long amount = 1);
    }
}
