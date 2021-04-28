using System.Threading.Tasks;

namespace Hinode.Izumi.Services.BackgroundJobs.EmoteJob
{
    public interface IEmoteJob
    {
        /// <summary>
        /// Загружает иконки в базу со всех серверов дискорда на которых присутствует Изуми. Сбрасывает кэш иконок.
        /// </summary>
        Task UploadEmotes();
    }
}
