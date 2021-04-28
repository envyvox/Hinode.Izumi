namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Иконка. Id модели = Id иконки.
    /// </summary>
    public class Emote : EntityBase
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
