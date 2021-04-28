namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Изображение.
    /// </summary>
    public class Image : EntityBase
    {
        /// <summary>
        /// Название изображения.
        /// </summary>
        public Enums.Image Type { get; set; }

        /// <summary>
        /// Url-ссылка на изображение.
        /// </summary>
        public string Url { get; set; }
    }
}
