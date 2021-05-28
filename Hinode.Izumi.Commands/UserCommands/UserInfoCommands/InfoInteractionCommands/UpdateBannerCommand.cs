using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.BannerService.Commands;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.InfoInteractionCommands
{
    [CommandCategory(CommandCategory.UserInfo, CommandCategory.UserInfoInteraction)]
    [IzumiRequireRegistry]
    public class UpdateBannerCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UpdateBannerCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("баннер"), Alias("banner")]
        [Summary("Изменить текущий баннер на указанный")]
        [CommandUsage("!баннер 1", "!баннер 5")]
        public async Task UpdateBannerTask(
            [Summary("Номер баннера")] long bannerId)
        {
            // получаем активный баннер пользователя
            var currentBanner = await _mediator.Send(new GetUserActiveBannerQuery((long) Context.User.Id));
            // получаем баннер который пользователь хочет установить как новый
            var banner = await _mediator.Send(new GetUserBannerQuery((long) Context.User.Id, bannerId));

            // новый баннер не может быть текущим баннером
            if (currentBanner.Id == banner.Id)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserBannerUpdateAlready.Parse(
                    banner.Rarity.Localize().ToLower(), banner.Name)));
            }
            else
            {
                // убираем статус активного с прошлого баннер
                await _mediator.Send(new DeactivateBannerInUserCommand((long) Context.User.Id, currentBanner.Id));
                // устанавливаем статус активного на новом баннере
                await _mediator.Send(new ActivateBannerInUserCommand((long) Context.User.Id, banner.Id));

                var embed = new EmbedBuilder()
                    // подверждаем что смена баннера прошла успешно
                    .WithDescription(IzumiReplyMessage.UserBannerUpdateSuccess.Parse(
                        banner.Rarity.Localize().ToLower(), banner.Name));

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
                await Task.CompletedTask;
            }
        }
    }
}
