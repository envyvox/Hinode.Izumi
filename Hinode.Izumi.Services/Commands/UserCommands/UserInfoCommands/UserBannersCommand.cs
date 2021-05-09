using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BannerService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserBannersCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IBannerService _bannerService;

        public UserBannersCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IBannerService bannerService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _bannerService = bannerService;
        }

        [Command("баннеры"), Alias("banners")]
        public async Task UserBannersTask()
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем все баннеры пользователя
            var userBanners = await _bannerService.GetUserBanner((long) Context.User.Id);

            var embed = new EmbedBuilder()
                // рассказываем как менять баннеры
                .WithDescription(
                    IzumiReplyMessage.UserBannerListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого баннера у пользователя создаем embed field
            foreach (var banner in userBanners)
            {
                // нельзя создать больше 25 embed field
                if (embed.Fields.Count == 25)
                {
                    embed.WithFooter("Тут отображены только первые 25 ваших баннеров.");
                    continue;
                }

                // добавляем информацию о баннере
                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{banner.Id}` {banner.Rarity.Localize()} «{banner.Name}»",
                    IzumiReplyMessage.UserBannerFieldDesc.Parse(banner.Url));
            }

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
