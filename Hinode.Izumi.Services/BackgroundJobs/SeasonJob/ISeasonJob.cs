using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.BackgroundJobs.SeasonJob
{
    public interface ISeasonJob
    {
        /// <summary>
        /// Оповещает о наступлении весны. Запускает джобу со сменой сезона через неделю.
        /// </summary>
        Task SpringComing();

        /// <summary>
        /// Оповещает о наступлении лета. Запускает джобу со сменой сезона через неделю.
        /// </summary>
        Task SummerComing();

        /// <summary>
        /// Оповещает о наступлении осени. Запускает джобу со сменой сезона через неделю.
        /// </summary>
        Task AutumnComing();

        /// <summary>
        /// Оповещает о наступлении зимы. Запускает джобу со сменой сезона через неделю.
        /// </summary>
        Task WinterComing();

        /// <summary>
        /// Сбрасывает все ячейки участков и обновляет текущий сезон в мире.
        /// </summary>
        /// <param name="season">Новый сезон.</param>
        Task UpdateSeason(Season season);
    }
}
