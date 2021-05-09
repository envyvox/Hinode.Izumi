using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.UserService
{
    public interface IUserService
    {
        /// <summary>
        /// Возвращает массив из топ-10 пользователей (сортировка по очкам приключений а затем по дате регистрации).
        /// </summary>
        /// <returns>Массив пользователей.</returns>
        Task<UserWithRowNumber[]> GetTopUsers();

        /// <summary>
        /// Возвращает модель пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Модель пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserWithId"></exception>
        Task<UserModel> GetUser(long userId);

        /// <summary>
        /// Возвращает модель пользователя.
        /// </summary>
        /// <param name="namePattern">Паттерн игрового имени пользователя.</param>
        /// <returns>Модель пользователя.</returns>
        /// <exception cref="IzumiNullableMessage.UserWithName"></exception>
        Task<UserModel> GetUser(string namePattern);

        /// <summary>
        /// Возвращает пользователя с позицией в рейтинге приключений.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Пользователь с позицией в рейтинге приключений.</returns>
        /// <exception cref="IzumiNullableMessage.UserWithId"></exception>
        Task<UserWithRowNumber> GetUserWithRowNumber(long userId);

        /// <summary>
        /// Возвращает пользователя с позицией в рейтинге приключений.
        /// </summary>
        /// <param name="namePattern">Паттерн игрового имени пользователя.</param>
        /// <returns>Пользователь с позицией в рейтинге приключений.</returns>
        /// <exception cref="IzumiNullableMessage.UserWithName"></exception>
        Task<UserWithRowNumber> GetUserWithRowNumber(string namePattern);

        /// <summary>
        /// Возвращает библиотеку титулов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека титулов пользователя.</returns>
        Task<Dictionary<Title, UserTitleModel>> GetUserTitle(long userId);

        /// <summary>
        /// Проверяет наличие записи с пользователем. Кэшируется.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>True есть есть, false если нет.</returns>
        Task<bool> CheckUser(long userId);

        /// <summary>
        /// Проверяет наличие записи с пользователем. Кэшируется.
        /// </summary>
        /// <param name="name">Игровое имя.</param>
        /// <returns>True есть есть, false если нет.</returns>
        Task<bool> CheckUser(string name);

        /// <summary>
        /// Добавляет пользователя в базу и кэш.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="name">Игровое имя.</param>
        Task AddUser(long userId, string name);

        /// <summary>
        /// Удаляет пользователя из базы и кэша.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        Task RemoveUser(long userId);

        /// <summary>
        /// Добавляет титул пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="title">Титул.</param>
        Task AddTitleToUser(long userId, Title title);

        /// <summary>
        /// Добавляет титул массиву пользователей.
        /// </summary>
        /// <param name="usersId">Массив id пользователей.</param>
        /// <param name="title">Титул.</param>
        Task AddTitleToUser(long[] usersId, Title title);

        /// <summary>
        /// Изменяет игровое имя пользователя на новое.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="name">Новое игровое имя.</param>
        Task UpdateUserName(long userId, string name);

        /// <summary>
        /// Изменяет описание пользователя на новое.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="about">Новое описание.</param>
        Task UpdateUserAbout(long userId, string about);

        /// <summary>
        /// Изменяет пол пользователя на новый.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="gender">Новый пол.</param>
        Task UpdateUserGender(long userId, Gender gender);

        /// <summary>
        /// Изменяет текущий титул пользователя на новый.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="title">Новый титул.</param>
        Task UpdateUserTitle(long userId, Title title);

        /// <summary>
        /// Добавляет энергию пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="amount">Количество энергии.</param>
        Task AddEnergyToUser(long userId, long amount);

        /// <summary>
        /// Добавляет энергию массиву пользователей.
        /// </summary>
        /// <param name="usersId">Массив Id пользователей.</param>
        /// <param name="amount">Количество энергии.</param>
        Task AddEnergyToUser(long[] usersId, long amount);

        /// <summary>
        /// Отнимает энергию у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="amount">Количество энергии.</param>
        Task RemoveEnergyFromUser(long userId, long amount);
    }
}
