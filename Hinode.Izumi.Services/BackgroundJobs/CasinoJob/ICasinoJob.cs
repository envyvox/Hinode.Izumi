using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.CasinoJob
{
    public interface ICasinoJob
    {
        /// <summary>
        /// Оповещает об открытии казино.
        /// </summary>
        Task Open();

        /// <summary>
        /// Оповещает об закрытии казино.
        /// </summary>
        Task Close();
    }
}
