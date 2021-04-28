namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Алкоголь.
    /// </summary>
    public class Alcohol : EntityBase
    {
        /// <summary>
        /// Название алкоголя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления.
        /// </summary>
        public long Time { get; set; }
    }
}
