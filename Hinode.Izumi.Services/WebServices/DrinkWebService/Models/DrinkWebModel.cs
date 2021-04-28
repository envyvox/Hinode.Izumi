using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.WebServices.DrinkWebService.Models
{
    /// <summary>
    /// Напиток.
    /// </summary>
    public class DrinkWebModel : EntityBaseModel
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
