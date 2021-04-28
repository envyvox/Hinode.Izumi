using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.WorldPropertyWebService.Models
{
    /// <summary>
    /// Свойства (настройки) игрового мира.
    /// </summary>
    public class WorldPropertyWebModel : EntityBaseModel
    {
        /// <summary>
        /// Категория свойства игрового мира.
        /// </summary>
        public PropertyCategory PropertyCategory { get; set; }

        /// <summary>
        /// Свойство игрового мира.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public long Value { get; set; }
    }
}
