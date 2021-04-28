using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.FieldService.Models;

namespace Hinode.Izumi.Services.RpgServices.FieldService
{
    public interface IFieldService
    {
        /// <summary>
        /// Возвращает клетку земли участка пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fieldId">Номер клетки земли.</param>
        /// <returns>Ячейка земли участка пользователя.</returns>
        Task<UserFieldModel> GetUserField(long userId, long fieldId);

        /// <summary>
        /// Возвращает массив клеток земли участка пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Массив клеток земли участка пользователя.</returns>
        Task<UserFieldModel[]> GetUserField(long userId);

        /// <summary>
        /// Добавляет клетки земли участка пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fieldsId">Массив номеров ячеек земли.</param>
        Task AddFieldToUser(long userId, long[] fieldsId);

        /// <summary>
        /// Добавляет семена на клетку земли участка пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fieldId">Номер клетки земли.</param>
        /// <param name="seedId">Id семени.</param>
        Task Plant(long userId, long fieldId, long seedId);

        /// <summary>
        /// Начинает повторный рост клетки земли участка пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fieldId">Номер клетки земли.</param>
        Task StartReGrowth(long userId, long fieldId);

        /// <summary>
        /// Обновляет состояние клетки земли участка пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="state">Состояние клетки земли.</param>
        Task UpdateState(long userId, FieldState state);

        /// <summary>
        /// Обновляет состояние всех клеток земли.
        /// </summary>
        /// <param name="state">Состояние клетки земли.</param>
        Task UpdateState(FieldState state);

        /// <summary>
        /// Добавляет прогресс роста клеток земли.
        /// </summary>
        Task MoveProgress();

        /// <summary>
        /// Сбрасывает состояние клетки земли участка пользователя к значению по-умолчанию.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="fieldId">Номер клетки земли.</param>
        Task ResetField(long userId, long fieldId);

        /// <summary>
        /// Сбрасывает состояние всех клеток земли к значению по-умолчанию.
        /// </summary>
        Task ResetField();
    }
}
