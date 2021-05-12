using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.FamilyService.Models;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.FamilyService
{
    public interface IFamilyService
    {
        /// <summary>
        /// Возвращает массив семей.
        /// </summary>
        /// <returns>Массив семей.</returns>
        Task<FamilyModel[]> GetAllFamilies();

        /// <summary>
        /// Возвращает семью.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <returns>Семья.</returns>
        Task<FamilyModel> GetFamily(long familyId);

        /// <summary>
        /// Возвращает семью.
        /// </summary>
        /// <param name="familyName">Название семьи.</param>
        /// <returns>Семья.</returns>
        Task<FamilyModel> GetFamily(string familyName);

        /// <summary>
        /// Возвращает семью пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Семья пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserFamily"></exception>
        Task<UserFamilyModel> GetUserFamily(long userId);

        /// <summary>
        /// Возвращает пользователя который является главой семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <returns>Пользователь, который является главой семьи.</returns>
        Task<UserModel> GetFamilyOwner(long familyId);

        /// <summary>
        /// Проверяет состоит ли пользователь в семье.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>True если да, false если нет.</returns>
        Task<bool> CheckUserHasFamily(long userId);

        /// <summary>
        /// Проверяет есть ли семья с таким названием.
        /// </summary>
        /// <param name="name">Название семьи.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckFamily(string name);

        /// <summary>
        /// Возвращает приглашение в семью.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Приглашение в семью.</returns>
        Task<FamilyInviteModel> GetFamilyInvite(long familyId, long userId);

        /// <summary>
        /// Возвращает приглашение в семью.
        /// </summary>
        /// <param name="inviteId">Id приглашения.</param>
        /// <returns>Приглашение в семью.</returns>
        /// <exception cref="IzumiNullableMessage.FamilyInvite"></exception>
        Task<FamilyInviteModel> GetFamilyInvite(long inviteId);

        /// <summary>
        /// Возвращает массив приглашений в семью.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <returns>Массив приглашений в семью.</returns>
        Task<FamilyInviteModel[]> GetFamilyInvites(long familyId);

        /// <summary>
        /// Возвращает массив приглашений в семью.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив приглашений в семью.</returns>
        Task<FamilyInviteModel[]> GetUserFamilyInvites(long userId);

        /// <summary>
        /// Возвращает валюту семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="currency">Валюта.</param>
        /// <returns>Валюта семьи.</returns>
        Task<FamilyCurrencyModel> GetFamilyCurrency(long familyId, Currency currency);

        /// <summary>
        /// Возвращает библиотеку валюты семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <returns>Библиотека валюты семьи.</returns>
        Task<Dictionary<Currency, FamilyCurrencyModel>> GetFamilyCurrency(long familyId);

        /// <summary>
        /// Добавляет семью.
        /// </summary>
        /// <param name="name">Название семьи.</param>
        Task AddFamily(string name);

        /// <summary>
        /// Добавляет пользователя в семью.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="familyId">Id семьи.</param>
        Task AddUserToFamily(long userId, long familyId);

        /// <summary>
        /// Добавляет пользователя в семью и назначает его главой семьи.
        /// (Используется при создании семьи!)
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="familyName">Название семьи.</param>
        Task AddUserToFamily(long userId, string familyName);

        /// <summary>
        /// Добавляет приглашение в семью.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="userId">Id пользователя.</param>
        Task AddFamilyInvite(long familyId, long userId);

        /// <summary>
        /// Добавляет валюту семье.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="currency">Валюта.</param>
        /// <param name="amount">Количество валюты.</param>
        Task AddCurrencyToFamily(long familyId, Currency currency, long amount);

        /// <summary>
        /// Обновляет название семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="familyName">Название семьи.</param>
        Task UpdateFamilyName(long familyId, string familyName);

        /// <summary>
        /// Обновляет описание семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="description">Описание семьи.</param>
        Task UpdateFamilyDescription(long familyId, string description);

        /// <summary>
        /// Обновляет статус пользователя в семье.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="status">Статус пользователя в семье.</param>
        Task UpdateUserInFamilyStatus(long userId, UserInFamilyStatus status);

        /// <summary>
        /// Удаляет семью.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        Task RemoveFamily(long familyId);

        /// <summary>
        /// Убирает пользователя из семьи.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        Task RemoveUserFromFamily(long userId);

        /// <summary>
        /// Удаляет приглашение в семью.
        /// </summary>
        /// <param name="inviteId">Id приглашения.</param>
        Task RemoveFamilyInvite(long inviteId);

        /// <summary>
        /// Забирает валюту у семьи.
        /// </summary>
        /// <param name="familyId">Id семьи.</param>
        /// <param name="currency">Валюта.</param>
        /// <param name="amount">Количество валюты.</param>
        Task RemoveCurrencyFromFamily(long familyId, Currency currency, long amount);

        /// <summary>
        /// Возвращает локализированный embed-builder с отображением информации о семье.
        /// </summary>
        /// <param name="embed">Embed builder.</param>
        /// <param name="family">Семья.</param>
        /// <returns>Локализированный embed-builder с отображением информации о семье.</returns>
        Task<EmbedBuilder> DisplayFamily(EmbedBuilder embed, FamilyModel family);
    }
}
