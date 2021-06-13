using System;

namespace Hinode.Izumi.Data.Enums.FamilyEnums
{
    public enum UserInFamilyStatus : byte
    {
        Default = 0,
        Deputy = 1,
        Head = 2
    }

    public static class UserInFamilyStatusHelper
    {
        public static string Localize(this UserInFamilyStatus status) => status switch
        {
            UserInFamilyStatus.Default => "участник семьи",
            UserInFamilyStatus.Deputy => "заместитель семьи",
            UserInFamilyStatus.Head => "глава семьи",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
