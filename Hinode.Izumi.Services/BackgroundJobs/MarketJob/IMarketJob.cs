using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.MarketJob
{
    public interface IMarketJob
    {
        /// <summary>
        /// Обновляет все заявки на рынке от Изуми.
        /// </summary>
        Task DailyMarketReset();
    }
}
