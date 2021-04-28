using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.CurrencyJob
{
    public interface ICurrencyJob
    {
        /// <summary>
        /// Добавляет иены всем пользователям у которых есть эффект ежедневного получения иен.
        /// </summary>
        /// <returns></returns>
        Task DailyIncome();
    }
}
