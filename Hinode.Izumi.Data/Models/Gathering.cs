using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Собирательский ресурс.
    /// </summary>
    public class Gathering : EntityBase
    {
        /// <summary>
        /// Название собирательского ресурса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена собирательского ресурса.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// Локация, в которой его можно получить.
        /// </summary>
        public Location Location { get; set; }
    }
}
