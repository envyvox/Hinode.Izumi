using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.AdministrationCommands
{
    [Group("preset")]
    [IzumiRequireContext(DiscordContext.Guild)]
    public class PresetMessageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;

        public PresetMessageCommands(IDiscordEmbedService discordEmbedService, IDiscordGuildService discordGuildService,
            IEmoteService emoteService, IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _emoteService = emoteService;
            _imageService = imageService;
        }

        [Command("game-roles")]
        public async Task SendGameRolesMessage()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем каналы дискорда
            var channels = await _discordGuildService.GetChannels();
            // получаем роли дискорда
            var roles = await _discordGuildService.GetRoles();
            // собираем сообщение
            var embed = new EmbedBuilder()
                .WithAuthor(IzumiReplyMessage.PresetGameRolesAuthor.Parse())
                .WithDescription(IzumiReplyMessage.PresetGameRolesDesc.Parse(channels[DiscordChannel.Search].Id))
                .AddField(IzumiReplyMessage.PresetGameRolesFieldName.Parse(),
                    IzumiReplyMessage.PresetGameRolesFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("GenshinImpact"), roles[DiscordRole.GenshinImpact].Id,
                        emotes.GetEmoteOrBlank("LeagueOfLegends"), roles[DiscordRole.LeagueOfLegends].Id,
                        emotes.GetEmoteOrBlank("TeamfightTactics"), roles[DiscordRole.TeamfightTactics].Id,
                        emotes.GetEmoteOrBlank("Valorant"), roles[DiscordRole.Valorant].Id,
                        emotes.GetEmoteOrBlank("ApexLegends"), roles[DiscordRole.ApexLegends].Id,
                        emotes.GetEmoteOrBlank("LostArk"), roles[DiscordRole.LostArk].Id,
                        emotes.GetEmoteOrBlank("Dota"), roles[DiscordRole.Dota].Id,
                        emotes.GetEmoteOrBlank("AmongUs"), roles[DiscordRole.AmongUs].Id,
                        emotes.GetEmoteOrBlank("Osu"), roles[DiscordRole.Osu].Id))
                .WithFooter(IzumiReplyMessage.PresetRolesFooter.Parse());

            // отправляем сообщение в канал
            var message = await ReplyAsync("", false, _discordEmbedService.BuildEmbed(embed));
            // добавляем реакции к сообщению
            await message.AddReactionsAsync(new IEmote[]
            {
                Emote.Parse(emotes.GetEmoteOrBlank("GenshinImpact")),
                Emote.Parse(emotes.GetEmoteOrBlank("LeagueOfLegends")),
                Emote.Parse(emotes.GetEmoteOrBlank("TeamfightTactics")),
                Emote.Parse(emotes.GetEmoteOrBlank("Valorant")),
                Emote.Parse(emotes.GetEmoteOrBlank("ApexLegends")),
                Emote.Parse(emotes.GetEmoteOrBlank("LostArk")),
                Emote.Parse(emotes.GetEmoteOrBlank("Dota")),
                Emote.Parse(emotes.GetEmoteOrBlank("AmongUs")),
                Emote.Parse(emotes.GetEmoteOrBlank("Osu"))
            });
        }

        [Command("registry")]
        public async Task SendAnonsRolesMessage()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем роли дискорда
            var roles = await _discordGuildService.GetRoles();

            // собираем сообщение
            var nicknameEmbed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.PresetRegistryNicknameTitle.Parse())
                .WithDescription(IzumiReplyMessage.PresetRegistryNicknameDesc.Parse())
                .WithImageUrl(await _imageService.GetImageUrl(Image.RegistryNicknames));

            // собираем сообщение
            var registryEmbed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.PresetRegistryCommandTitle.Parse())
                .WithDescription(IzumiReplyMessage.PresetRegistryCommandDesc.Parse())
                .WithImageUrl(await _imageService.GetImageUrl(Image.RegistryCommand));

            // собираем сообщение
            var anonsRolesEmbed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.PresetRegistryAnonsRolesTitle.Parse())
                .WithDescription(IzumiReplyMessage.PresetRegistryAnonsRolesDesc.Parse())
                .AddField(IzumiReplyMessage.PresetRegistryAnonsRolesFieldName.Parse(),
                    IzumiReplyMessage.PresetRegistryAnonsRolesFieldDesc.Parse(
                        emotes.GetEmoteOrBlank("NumOne"), roles[DiscordRole.AllEvents].Id,
                        emotes.GetEmoteOrBlank("NumTwo"), roles[DiscordRole.DailyEvents].Id,
                        emotes.GetEmoteOrBlank("NumThree"), roles[DiscordRole.WeeklyEvents].Id,
                        emotes.GetEmoteOrBlank("NumFour"), roles[DiscordRole.MonthlyEvents].Id,
                        emotes.GetEmoteOrBlank("NumFive"), roles[DiscordRole.YearlyEvents].Id,
                        emotes.GetEmoteOrBlank("NumSix"), roles[DiscordRole.UniqueEvents].Id))
                .WithFooter(IzumiReplyMessage.PresetRolesFooter.Parse())
                .WithImageUrl(await _imageService.GetImageUrl(Image.RegistryGetAnonsRoles));

            // отправляем сообщение в канал
            await _discordEmbedService.SendEmbed(Context.Channel, nicknameEmbed);
            // отправляем сообщение в канал
            await _discordEmbedService.SendEmbed(Context.Channel, registryEmbed);

            // отправляем сообщение в канал
            var message = await ReplyAsync("", false, _discordEmbedService.BuildEmbed(anonsRolesEmbed));
            // добавляем реакции к сообщению
            await message.AddReactionsAsync(new IEmote[]
            {
                Emote.Parse(emotes.GetEmoteOrBlank("NumOne")),
                Emote.Parse(emotes.GetEmoteOrBlank("NumTwo")),
                Emote.Parse(emotes.GetEmoteOrBlank("NumThree")),
                Emote.Parse(emotes.GetEmoteOrBlank("NumFour")),
                Emote.Parse(emotes.GetEmoteOrBlank("NumFive")),
                Emote.Parse(emotes.GetEmoteOrBlank("NumSix"))
            });
        }
    }
}
