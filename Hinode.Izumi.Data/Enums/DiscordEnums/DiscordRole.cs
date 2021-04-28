using System;

namespace Hinode.Izumi.Data.Enums.DiscordEnums
{
    /// <summary>
    /// Роль на сервере дискорда.
    /// </summary>
    public enum DiscordRole
    {
        MusicBot = 1,
        Administration = 2,
        EventManager = 3,
        Moderator = 4,
        Nitro = 5, // роль nitro-boost создается дискордом по-умолчанию, нам нужно только получить ее
        Mute = 6,

        LocationInTransit = 50,
        LocationCapital = 51,
        LocationGarden = 52,
        LocationSeaport = 53,
        LocationCastle = 54,
        LocationVillage = 55,

        AllEvents = 100,
        DailyEvents = 101,
        WeeklyEvents = 102,
        MonthlyEvents = 103,
        YearlyEvents = 104,
        UniqueEvents = 105,

        GenshinImpact = 200,
        LeagueOfLegends = 201,
        TeamfightTactics = 202,
        Valorant = 203,
        ApexLegends = 204,
        LostArk = 205,
        Dota = 206,
        Osu = 207,
        AmongUs = 208,
    }

    public static class DiscordRoleHelper
    {
        /// <summary>
        /// Возвращает локализированное название роли.
        /// </summary>
        /// <param name="role">Роль.</param>
        /// <returns>Локализированное название роли.</returns>
        public static string Name(this DiscordRole role) => role switch
        {
            DiscordRole.MusicBot => "Музыкальные боты",
            DiscordRole.Administration => "Сёгунат",
            DiscordRole.EventManager => "Собаёри",
            DiscordRole.Moderator => "Родзю",
            DiscordRole.Nitro => "🤝 Поддержка сервера",
            DiscordRole.LocationInTransit => "🐫 " + Location.InTransit.Localize(),
            DiscordRole.LocationCapital => "🏯 " + Location.Capital.Localize(),
            DiscordRole.LocationGarden => "🌳 " + Location.Garden.Localize(),
            DiscordRole.LocationSeaport => "⛵ " + Location.Seaport.Localize(),
            DiscordRole.LocationCastle => "🏰 " + Location.Castle.Localize(),
            DiscordRole.LocationVillage => "🎑 " + Location.Village.Localize(),
            DiscordRole.AllEvents => "📅 Все события",
            DiscordRole.DailyEvents => "📅 Ежедневные события",
            DiscordRole.WeeklyEvents => "📅 Еженедельные события",
            DiscordRole.MonthlyEvents => "📅 Ежемесячные события",
            DiscordRole.YearlyEvents => "📅 Ежегодные события",
            DiscordRole.UniqueEvents => "📅 Уникальные события",
            DiscordRole.GenshinImpact => "Genshin Impact",
            DiscordRole.LeagueOfLegends => "League of Legends",
            DiscordRole.TeamfightTactics => "Teamfight Tactics",
            DiscordRole.Valorant => "Valorant",
            DiscordRole.ApexLegends => "Apex Legends",
            DiscordRole.LostArk => "LostArk",
            DiscordRole.Dota => "Dota 2",
            DiscordRole.Osu => "Osu!",
            DiscordRole.AmongUs => "Among Us",
            DiscordRole.Mute => "Блокировка чата",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };

        /// <summary>
        /// Возвращает hex-цвет роли.
        /// </summary>
        /// <param name="role">Роль.</param>
        /// <returns>Hex-цвет роли.</returns>
        public static string Color(this DiscordRole role) => role switch
        {
            DiscordRole.Administration => "ffc7f5",
            DiscordRole.EventManager => "e99edb",
            DiscordRole.Moderator => "c072b2",
            DiscordRole.Nitro => "f47fff",
            // для всех остальных используем значение по-умолчанию (прозрачный цвет дискорда)
            _ => "000000"
        };
    }
}
