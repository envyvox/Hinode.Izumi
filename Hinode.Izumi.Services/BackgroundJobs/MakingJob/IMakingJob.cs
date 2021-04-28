using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.BackgroundJobs.MakingJob
{
    public interface IMakingJob
    {
        /// <summary>
        /// Завершает изготовление предмета.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <param name="amount">Количество изготавливаемых предметов.</param>
        /// <param name="location">Локация пользователя до начала изготовления.</param>
        Task CompleteCrafting(long userId, long craftingId, long amount, Location location);

        /// <summary>
        /// Завершает изготовление алкоголя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <param name="amount">Количество изготавливаемого алкоголя.</param>
        /// <param name="location">Локация пользователя до начала изготовления.</param>
        Task CompleteAlcohol(long userId, long alcoholId, long amount, Location location);

        /// <summary>
        /// Завершает приготовление блюда.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="foodId">Id блюда.</param>
        /// <param name="amount">Количество приготавливаемого блюда.</param>
        /// <param name="location">Локация пользователя до начала приготовления.</param>
        Task CompleteFood(long userId, long foodId, long amount, Location location);
    }
}
