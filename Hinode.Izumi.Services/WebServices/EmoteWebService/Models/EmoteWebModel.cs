using System;

namespace Hinode.Izumi.Services.WebServices.EmoteWebService.Models
{
    /// <summary>
    /// Иконка. Id модели = Id иконки.
    /// </summary>
    public class EmoteWebModel
    {
        /// <summary>
        /// Id иконки.
        /// Хранится в строке потому что
        /// https://stackoverflow.com/questions/1379934/large-numbers-erroneously-rounded-in-javascript.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название иконки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полный код иконки в формате :название:id.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Время создания записи.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Время последнего обновления записи.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
