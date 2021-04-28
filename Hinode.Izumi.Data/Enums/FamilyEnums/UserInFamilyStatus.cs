using System;

namespace Hinode.Izumi.Data.Enums.FamilyEnums
{
    /// <summary>
    /// Статус пользователя в семье.
    /// </summary>
    public enum UserInFamilyStatus
    {
        Default = 0,
        Deputy = 1,
        Head = 2
    }

    public static class UserInFamilyStatusHelper
    {
        /// <summary>
        /// Возвращает локализированное название статуса пользователя в семье.
        /// </summary>
        /// <param name="status">Статус пользователя в семье.</param>
        /// <returns>Локализированное название статуса пользователя в семье.</returns>
        public static string Localize(this UserInFamilyStatus status) => status switch
        {
            UserInFamilyStatus.Default => "участник семьи",
            UserInFamilyStatus.Deputy => "заместитель семьи",
            UserInFamilyStatus.Head => "глава семьи",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
