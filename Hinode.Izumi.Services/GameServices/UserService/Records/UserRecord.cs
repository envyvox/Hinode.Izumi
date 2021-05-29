using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.UserService.Records
{
    public record UserRecord(
        long Id,
        string Name,
        string About,
        Title Title,
        Gender Gender,
        Location Location,
        int Energy,
        long Points,
        DateTimeOffset CreatedAt)
    {
        public UserRecord() : this(default, default, default, default, default, default, default, default, default)
        {
        }
    }
}
