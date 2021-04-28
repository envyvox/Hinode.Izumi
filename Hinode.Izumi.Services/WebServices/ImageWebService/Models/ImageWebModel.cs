using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.ImageWebService.Models
{
    /// <summary>
    /// Изображение.
    /// </summary>
    public class ImageWebModel : EntityBaseModel
    {
        /// <summary>
        /// Название изображения.
        /// </summary>
        public Image Type { get; set; }

        /// <summary>
        /// Url-ссылка на изображение.
        /// </summary>
        public string Url { get; set; }
    }
}
