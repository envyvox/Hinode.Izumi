using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Изготавливаемый предмет.
    /// </summary>
    public class Crafting : EntityBase
    {
        /// <summary>
        /// Название Изготавливаемого предмета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления этого предмета.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Локация, в которой можно изготовить этот предмет.
        /// </summary>
        public Location Location { get; set; }
    }
}
