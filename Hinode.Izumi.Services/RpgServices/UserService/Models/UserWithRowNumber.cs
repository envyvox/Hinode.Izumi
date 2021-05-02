namespace Hinode.Izumi.Services.RpgServices.UserService.Models
{
    /// <summary>
    /// Пользователь с номером в списке.
    /// </summary>
    public class UserWithRowNumber : UserModel
    {
        /// <summary>
        /// Номер в списке.
        /// </summary>
        public long RowNumber { get; set; }
    }
}
