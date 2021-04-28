using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ContractService.Models
{
    /// <summary>
    /// Рабочий контракт.
    /// </summary>
    public class ContractModel : EntityBaseModel
    {
        /// <summary>
        /// Локация, в которой доступен этот контракт.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Название контракта.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание контракта.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Длительность работы в минутах.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Количество иен за выполнение контракта.
        /// </summary>
        public long Currency { get; set; }

        /// <summary>
        /// Количество репутации за выполнение контракта.
        /// </summary>
        public long Reputation { get; set; }

        /// <summary>
        /// Количество затрачиваемой энергии при работе по контракту.
        /// </summary>
        public long Energy { get; set; }
    }
}
