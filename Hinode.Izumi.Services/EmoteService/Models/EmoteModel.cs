using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.EmoteService.Models
{
    public class EmoteModel : EntityBaseModel
    {
        /// <summary>
        /// Название иконки.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полный код иконки в формате :название:id.
        /// </summary>
        public string Code { get; set; }
    }
}
