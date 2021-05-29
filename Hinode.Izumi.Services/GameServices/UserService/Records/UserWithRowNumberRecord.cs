using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.GameServices.UserService.Records
{
    public record UserWithRowNumberRecord(
        long Id,
        string Name,
        string About,
        Title Title,
        Gender Gender,
        Location Location,
        int Energy,
        long Points,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt,
        long RowNumber)
    {
        public UserWithRowNumberRecord() : this(
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default,
            default)
        {
        }
    }
}
