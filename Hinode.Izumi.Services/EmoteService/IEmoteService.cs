using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.EmoteService.Models;

namespace Hinode.Izumi.Services.EmoteService
{
    public interface IEmoteService
    {
        /// <summary>
        /// Возвращает библиотеку иконок из базы. Кэшируется.
        /// </summary>
        /// <returns>Библиотека иконок.</returns>
        Task<Dictionary<string, EmoteModel>> GetEmotes();

        /// <summary>
        /// Загружает иконки в базу со всех серверов дискорда на которых присутствует Изуми. Сбрасывает кэш иконок.
        /// </summary>
        Task UploadEmotes();
    }
}
