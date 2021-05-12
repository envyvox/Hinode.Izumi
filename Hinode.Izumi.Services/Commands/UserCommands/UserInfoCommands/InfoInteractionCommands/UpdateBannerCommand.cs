using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.BannerService;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UpdateBannerCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IBannerService _bannerService;

        public UpdateBannerCommand(IDiscordEmbedService discordEmbedService, IBannerService bannerService)
        {
            _discordEmbedService = discordEmbedService;
            _bannerService = bannerService;
        }

        [Command("баннер"), Alias("banner")]
        [Summary("Изменить текущий баннер на указанный")]
        [CommandUsage("!баннер 1", "!баннер 5")]
        public async Task UpdateBannerTask(
            [Summary("Номер баннера")] long bannerId)
        {
            // получаем активный баннер пользователя
            var currentBanner = await _bannerService.GetUserActiveBanner((long) Context.User.Id);
            // получаем баннер который пользователь хочет установить как новый
            var banner = await _bannerService.GetUserBanner((long) Context.User.Id, bannerId);

            // новый баннер не может быть текущим баннером
            if (currentBanner.Id == banner.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserBannerUpdateAlready.Parse(
                    banner.Rarity.Localize().ToLower(), banner.Name)));
            }
            else
            {
                // убираем статус активного с прошлого баннер
                await _bannerService.ToggleBannerInUser((long) Context.User.Id, currentBanner.Id, false);
                // устанавливаем статус активного на новом баннере
                await _bannerService.ToggleBannerInUser((long) Context.User.Id, banner.Id, true);

                var embed = new EmbedBuilder()
                    // подверждаем что смена баннера прошла успешно
                    .WithDescription(IzumiReplyMessage.UserBannerUpdateSuccess.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name));

                await _discordEmbedService.SendEmbed(Context.User, embed);
                await Task.CompletedTask;
            }
        }
    }
}
