using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.LocalizationWebService.Models
{
    /// <summary>
    /// Локализация.
    /// </summary>
    public class LocalizationWebModel : EntityBaseModel
    {
        /// <summary>
        /// Категория локализации.
        /// </summary>
        public LocalizationCategory Category { get; set; }

        /// <summary>
        /// Название предмета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Локализация предмета (1шт.)
        /// </summary>
        public string Single { get; set; }

        /// <summary>
        /// Локализация предмета (2шт.)
        /// </summary>
        public string Double { get; set; }

        /// <summary>
        /// Локализация предмета (5шт.)
        /// </summary>
        public string Multiply { get; set; }
    }
}
