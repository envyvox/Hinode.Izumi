using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.AdministrationCommands
{
    [Group("preset")]
    [IzumiRequireRole(DiscordRole.Administration)]
    public class PresetMessageCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public PresetMessageCommands(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("game-roles")]
        public async Task SendGameRolesMessage()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var channels = await _mediator.Send(new GetDiscordChannelsQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

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
                        emotes.GetEmoteOrBlank("Osu"), roles[DiscordRole.Osu].Id,
                        emotes.GetEmoteOrBlank("Rust"), roles[DiscordRole.Rust].Id,
                        emotes.GetEmoteOrBlank("CSGO"), roles[DiscordRole.CsGo].Id,
                        emotes.GetEmoteOrBlank("HotS"), roles[DiscordRole.HotS].Id,
                        emotes.GetEmoteOrBlank("WildRift"), roles[DiscordRole.WildRift].Id,
                        emotes.GetEmoteOrBlank("MobileLegends"), roles[DiscordRole.MobileLegends].Id))
                .WithFooter(IzumiReplyMessage.PresetRolesFooter.Parse());

            var message = await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.GetRoles, embed));
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
                Emote.Parse(emotes.GetEmoteOrBlank("Osu")),
                Emote.Parse(emotes.GetEmoteOrBlank("Rust")),
                Emote.Parse(emotes.GetEmoteOrBlank("CSGO")),
                Emote.Parse(emotes.GetEmoteOrBlank("HotS")),
                Emote.Parse(emotes.GetEmoteOrBlank("WildRift")),
                Emote.Parse(emotes.GetEmoteOrBlank("MobileLegends"))
            });
        }

        [Command("registry")]
        public async Task SendAnonsRolesMessage()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var roles = await _mediator.Send(new GetDiscordRolesQuery());

            var nicknameEmbed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.PresetRegistryNicknameTitle.Parse())
                .WithDescription(IzumiReplyMessage.PresetRegistryNicknameDesc.Parse(
                    emotes.GetEmoteOrBlank("List")))
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.RegistryNicknames)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Registration, nicknameEmbed));

            var registryEmbed = new EmbedBuilder()
                .WithTitle(IzumiReplyMessage.PresetRegistryCommandTitle.Parse())
                .WithDescription(IzumiReplyMessage.PresetRegistryCommandDesc.Parse())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.RegistryCommand)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Registration, registryEmbed));

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
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.RegistryGetAnonsRoles)));

            var message =
                await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Registration, anonsRolesEmbed));
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
