namespace Hinode.Izumi.Data.Models
{
    /// <summary>
    /// Напиток.
    /// </summary>
    public class Drink : EntityBase
    {
        /// <summary>
        /// Название напитка.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Длительность изготовления.
        /// </summary>
        public long Time { get; set; }
    }
}
