using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ImageService.Models
{
    /// <summary>
    /// Изображение.
    /// </summary>
    public class ImageModel : EntityBaseModel
    {
        /// <summary>
        /// Название изображения.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url-ссылка на изображение.
        /// </summary>
        public string Url { get; set; }
    }
}
