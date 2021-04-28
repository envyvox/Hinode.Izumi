﻿using System;

namespace Hinode.Izumi.Data.Enums.DiscordEnums
{
    /// <summary>
    /// Канал на сервере дискорда.
    /// </summary>
    public enum DiscordChannel
    {
        Chat = 1,
        SearchParent = 2,
        GetRoles = 3,
        Search = 4,
        GameWorldParent = 5,
        Updates = 6,
        Registration = 7,
        Diary = 8,
        CommunityDescParent = 9,
        Anons = 10,
        Giveaways = 11,
        Suggestions = 12,
        Memes = 13,
        Arts = 14,
        Erotic = 15,
        Nsfw = 16,
        LibraryParent = 17,
        Rules = 18,
        CreateRoomParent = 19,
        CreateRoom = 20,
        FamilyRoomParent = 21,
        CapitalParent = 22,
        CapitalDesc = 23,
        CapitalWhatToDo = 24,
        CapitalEvents = 25,
        GardenParent = 26,
        GardenDesc = 27,
        GardenWhatToDo = 28,
        GardenEvents = 29,
        SeaportParent = 30,
        SeaportDesc = 31,
        SeaportWhatToDo = 32,
        SeaportEvents = 33,
        CastleParent = 34,
        CastleDesc = 35,
        CastleWhatToDo = 36,
        CastleEvents = 37,
        VillageParent = 38,
        VillageDesc = 39,
        VillageWhatToDo = 40,
        VillageEvents = 41,
        AfkParent = 42,
        AfkRoom = 43,
        AdministrationParent = 44,
        AdministrationChat = 45,
        EventManagerChat = 46,
        ModeratorChat = 47,
        MeetingRoom = 48,
        TechnicalParent = 49,
        Test = 50,
        LogWelcome = 51,
        LogMute = 52,
        LogAudit = 53
    }

    public static class DiscordChannelHelper
    {
        private const string LocationDesc = "описание";
        private const string LocationWhatToDo = "чем-заняться";
        private const string LocationEvents = "события";

        /// <summary>
        /// Возвращает локализированное название канала.
        /// </summary>
        /// <param name="channel">Канал.</param>
        /// <returns>Локализированное название канала.</returns>
        public static string Name(this DiscordChannel channel) => channel switch
        {
            DiscordChannel.Chat => "общение",
            DiscordChannel.SearchParent => "поиск игроков",
            DiscordChannel.GetRoles => "получение-ролей",
            DiscordChannel.Search => "поиск-напарников",
            DiscordChannel.GameWorldParent => "игровая вселенная",
            DiscordChannel.Updates => "обновления🔔",
            DiscordChannel.Registration => "регистрация",
            DiscordChannel.Diary => "дневник-странницы",
            DiscordChannel.CommunityDescParent => "доска сообщества",
            DiscordChannel.Anons => "объявления🔔",
            DiscordChannel.Giveaways => "розыгрыши🔔",
            DiscordChannel.Suggestions => "предложения",
            DiscordChannel.Memes => "мемесы",
            DiscordChannel.Arts => "арты",
            DiscordChannel.Erotic => "эротика",
            DiscordChannel.Nsfw => "nsfw",
            DiscordChannel.LibraryParent => "великая «Тосёкан»",
            DiscordChannel.Rules => "правила",
            DiscordChannel.CreateRoomParent => "пригородные лагеря",
            DiscordChannel.CreateRoom => "Разжечь костер",
            DiscordChannel.FamilyRoomParent => "семейные беседки",
            DiscordChannel.CapitalParent => Location.Capital.Localize(),
            DiscordChannel.CapitalDesc => "🏯" + LocationDesc,
            DiscordChannel.CapitalWhatToDo => "🏯" + LocationWhatToDo,
            DiscordChannel.CapitalEvents => "🏯" + LocationEvents,
            DiscordChannel.GardenParent => Location.Garden.Localize(),
            DiscordChannel.GardenDesc => "🌳" + LocationDesc,
            DiscordChannel.GardenWhatToDo => "🌳" + LocationWhatToDo,
            DiscordChannel.GardenEvents => "🌳" + LocationEvents,
            DiscordChannel.SeaportParent => Location.Seaport.Localize(),
            DiscordChannel.SeaportDesc => "⛵" + LocationDesc,
            DiscordChannel.SeaportWhatToDo => "⛵" + LocationWhatToDo,
            DiscordChannel.SeaportEvents => "⛵" + LocationEvents,
            DiscordChannel.CastleParent => Location.Castle.Localize(),
            DiscordChannel.CastleDesc => "🏰" + LocationDesc,
            DiscordChannel.CastleWhatToDo => "🏰" + LocationWhatToDo,
            DiscordChannel.CastleEvents => "🏰" + LocationEvents,
            DiscordChannel.VillageParent => Location.Village.Localize(),
            DiscordChannel.VillageDesc => "🎑" + LocationDesc,
            DiscordChannel.VillageWhatToDo => "🎑" + LocationWhatToDo,
            DiscordChannel.VillageEvents => "🎑" + LocationEvents,
            DiscordChannel.AfkParent => "zzz",
            DiscordChannel.AfkRoom => "Афк, жду подарки",
            DiscordChannel.AdministrationParent => "административный раздел",
            DiscordChannel.AdministrationChat => "сёгунат",
            DiscordChannel.EventManagerChat => "собаёри",
            DiscordChannel.ModeratorChat => "родзю",
            DiscordChannel.MeetingRoom => "Собрание",
            DiscordChannel.TechnicalParent => "технический раздел",
            DiscordChannel.Test => "тестовый",
            DiscordChannel.LogWelcome => "log-welcome",
            DiscordChannel.LogMute => "log-mute",
            DiscordChannel.LogAudit => "log-audit",
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        /// <summary>
        /// Возвращает категорию к которой относится канал.
        /// </summary>
        /// <param name="channel">Канал.</param>
        /// <returns>Категория канала.</returns>
        public static DiscordChannelCategory Category(this DiscordChannel channel) => channel switch
        {
            DiscordChannel.Chat => DiscordChannelCategory.TextChannel,
            DiscordChannel.SearchParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.GetRoles => DiscordChannelCategory.TextChannel,
            DiscordChannel.Search => DiscordChannelCategory.TextChannel,
            DiscordChannel.GameWorldParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.Updates => DiscordChannelCategory.TextChannel,
            DiscordChannel.Registration => DiscordChannelCategory.TextChannel,
            DiscordChannel.Diary => DiscordChannelCategory.TextChannel,
            DiscordChannel.CommunityDescParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.Anons => DiscordChannelCategory.TextChannel,
            DiscordChannel.Giveaways => DiscordChannelCategory.TextChannel,
            DiscordChannel.Suggestions => DiscordChannelCategory.TextChannel,
            DiscordChannel.Memes => DiscordChannelCategory.TextChannel,
            DiscordChannel.Arts => DiscordChannelCategory.TextChannel,
            DiscordChannel.Erotic => DiscordChannelCategory.TextChannel,
            DiscordChannel.Nsfw => DiscordChannelCategory.TextChannel,
            DiscordChannel.LibraryParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.Rules => DiscordChannelCategory.TextChannel,
            DiscordChannel.CreateRoomParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.CreateRoom => DiscordChannelCategory.VoiceChannel,
            DiscordChannel.FamilyRoomParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.CapitalParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.CapitalDesc => DiscordChannelCategory.TextChannel,
            DiscordChannel.CapitalWhatToDo => DiscordChannelCategory.TextChannel,
            DiscordChannel.CapitalEvents => DiscordChannelCategory.TextChannel,
            DiscordChannel.GardenParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.GardenDesc => DiscordChannelCategory.TextChannel,
            DiscordChannel.GardenWhatToDo => DiscordChannelCategory.TextChannel,
            DiscordChannel.GardenEvents => DiscordChannelCategory.TextChannel,
            DiscordChannel.SeaportParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.SeaportDesc => DiscordChannelCategory.TextChannel,
            DiscordChannel.SeaportWhatToDo => DiscordChannelCategory.TextChannel,
            DiscordChannel.SeaportEvents => DiscordChannelCategory.TextChannel,
            DiscordChannel.CastleParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.CastleDesc => DiscordChannelCategory.TextChannel,
            DiscordChannel.CastleWhatToDo => DiscordChannelCategory.TextChannel,
            DiscordChannel.CastleEvents => DiscordChannelCategory.TextChannel,
            DiscordChannel.VillageParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.VillageDesc => DiscordChannelCategory.TextChannel,
            DiscordChannel.VillageWhatToDo => DiscordChannelCategory.TextChannel,
            DiscordChannel.VillageEvents => DiscordChannelCategory.TextChannel,
            DiscordChannel.AfkParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.AfkRoom => DiscordChannelCategory.VoiceChannel,
            DiscordChannel.AdministrationParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.AdministrationChat => DiscordChannelCategory.TextChannel,
            DiscordChannel.EventManagerChat => DiscordChannelCategory.TextChannel,
            DiscordChannel.ModeratorChat => DiscordChannelCategory.TextChannel,
            DiscordChannel.MeetingRoom => DiscordChannelCategory.VoiceChannel,
            DiscordChannel.TechnicalParent => DiscordChannelCategory.CategoryChannel,
            DiscordChannel.Test => DiscordChannelCategory.TextChannel,
            DiscordChannel.LogWelcome => DiscordChannelCategory.TextChannel,
            DiscordChannel.LogMute => DiscordChannelCategory.TextChannel,
            DiscordChannel.LogAudit => DiscordChannelCategory.TextChannel,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

        /// <summary>
        /// Возвращает родительсьский канал.
        /// </summary>
        /// <param name="channel">Канал.</param>
        /// <returns>Родительский канал.</returns>
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        public static DiscordChannel Parent(this DiscordChannel channel) => channel switch
        {
            DiscordChannel.GetRoles => DiscordChannel.SearchParent,
            DiscordChannel.Search => DiscordChannel.SearchParent,
            DiscordChannel.Updates => DiscordChannel.GameWorldParent,
            DiscordChannel.Registration => DiscordChannel.GameWorldParent,
            DiscordChannel.Diary => DiscordChannel.GameWorldParent,
            DiscordChannel.Anons => DiscordChannel.CommunityDescParent,
            DiscordChannel.Giveaways => DiscordChannel.CommunityDescParent,
            DiscordChannel.Suggestions => DiscordChannel.CommunityDescParent,
            DiscordChannel.Memes => DiscordChannel.CommunityDescParent,
            DiscordChannel.Arts => DiscordChannel.CommunityDescParent,
            DiscordChannel.Erotic => DiscordChannel.CommunityDescParent,
            DiscordChannel.Nsfw => DiscordChannel.CommunityDescParent,
            DiscordChannel.Rules => DiscordChannel.LibraryParent,
            DiscordChannel.CreateRoom => DiscordChannel.CreateRoomParent,
            DiscordChannel.FamilyRoomParent => DiscordChannel.CreateRoomParent,
            DiscordChannel.CapitalDesc => DiscordChannel.CapitalParent,
            DiscordChannel.CapitalWhatToDo => DiscordChannel.CapitalParent,
            DiscordChannel.CapitalEvents => DiscordChannel.CapitalParent,
            DiscordChannel.GardenDesc => DiscordChannel.GardenParent,
            DiscordChannel.GardenWhatToDo => DiscordChannel.GardenParent,
            DiscordChannel.GardenEvents => DiscordChannel.GardenParent,
            DiscordChannel.SeaportDesc => DiscordChannel.SeaportParent,
            DiscordChannel.SeaportWhatToDo => DiscordChannel.SeaportParent,
            DiscordChannel.SeaportEvents => DiscordChannel.SeaportParent,
            DiscordChannel.CastleDesc => DiscordChannel.CastleParent,
            DiscordChannel.CastleWhatToDo => DiscordChannel.CastleParent,
            DiscordChannel.CastleEvents => DiscordChannel.CastleParent,
            DiscordChannel.VillageDesc => DiscordChannel.VillageParent,
            DiscordChannel.VillageWhatToDo => DiscordChannel.VillageParent,
            DiscordChannel.VillageEvents => DiscordChannel.VillageParent,
            DiscordChannel.AfkRoom => DiscordChannel.AfkParent,
            DiscordChannel.AdministrationChat => DiscordChannel.AdministrationParent,
            DiscordChannel.EventManagerChat => DiscordChannel.AdministrationParent,
            DiscordChannel.ModeratorChat => DiscordChannel.AdministrationParent,
            DiscordChannel.MeetingRoom => DiscordChannel.AdministrationParent,
            DiscordChannel.Test => DiscordChannel.TechnicalParent,
            DiscordChannel.LogWelcome => DiscordChannel.TechnicalParent,
            DiscordChannel.LogMute => DiscordChannel.TechnicalParent,
            DiscordChannel.LogAudit => DiscordChannel.TechnicalParent,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }
}
