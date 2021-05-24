using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.BannerService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireRegistry]
    public class UserBannersCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserBannersCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("баннеры"), Alias("banners")]
        [Summary("Посмотреть свою коллекцию баннеров")]
        public async Task UserBannersTask()
        {
            // получаем все иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем все баннеры пользователя
            var userBanners = await _mediator.Send(new GetUserBannersQuery((long) Context.User.Id));

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

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
