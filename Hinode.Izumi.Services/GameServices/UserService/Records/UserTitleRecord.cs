using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.UserService.Records
{
    public record UserTitleRecord(long UserId, Title Title)
    {
        public UserTitleRecord() : this(default, default)
        {
        }
    }
}
