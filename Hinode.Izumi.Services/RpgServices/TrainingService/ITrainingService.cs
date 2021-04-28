using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.RpgServices.TrainingService
{
    public interface ITrainingService
    {
        /// <summary>
        /// Возвращает текущий шаг обучения пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Текущий шаг обучения пользователя.</returns>
        Task<TrainingStep> GetUserTrainingStep(long userId);

        /// <summary>
        /// Обновляет шаг обучения пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="step">Шаг обучения.</param>
        Task UpdateUserTrainingStep(long userId, TrainingStep step);

        /// <summary>
        /// Проверяет нужно ли продвинуть прогресс обучения пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="step">Шаг обучения.</param>
        Task CheckStep(long userId, TrainingStep step);
    }
}
