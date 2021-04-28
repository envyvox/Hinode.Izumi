using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.EnergyJob
{
    public interface IEnergyJob
    {
        /// <summary>
        /// Добавляет +1 энергии всем пользователям, у которых меньше 100 энергии.
        /// </summary>
        Task HourlyRecovery();
    }
}
