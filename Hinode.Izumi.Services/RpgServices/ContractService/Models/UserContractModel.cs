using Hinode.Izumi.Services.Database;

namespace Hinode.Izumi.Services.RpgServices.ContractService.Models
{
    /// <summary>
    /// Контракт которым занимается пользователь.
    /// </summary>
    public class UserContractModel : EntityBaseModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Id контракта.
        /// </summary>
        public long ContractId { get; set; }
    }
}
