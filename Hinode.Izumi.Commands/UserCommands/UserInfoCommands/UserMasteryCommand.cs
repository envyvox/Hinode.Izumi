using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands
{
    [CommandCategory(CommandCategory.UserInfo)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class UserMasteryCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public UserMasteryCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("мастерство"), Alias("mastery")]
        [Summary("Посмотреть информацию о своем мастерстве")]
        public async Task UserMasteryTask()
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем мастерство пользователя
            var userMasteries = await _mediator.Send(new GetUserMasteriesQuery((long) Context.User.Id));
            // получаем максимальное мастерство пользователя
            var userMaxMastery = await _mediator.Send(new GetUserMaxMasteryQuery((long) Context.User.Id));

            var embed = new EmbedBuilder()
                // рассказываем про лимит мастерства
                .WithDescription(
                    IzumiReplyMessage.UserMasteryDesc.Parse(userMaxMastery) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер мастерства
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Mastery)));

            // создаем embed field для каждого мастерства
            foreach (var mastery in Enum.GetValues(typeof(Mastery))
                .Cast<Mastery>())
            {
                // получаем округленное количество мастерства пользователя
                var userMasteryAmount = Math.Round(
                    userMasteries.ContainsKey(mastery)
                        ? userMasteries[mastery].Amount
                        : 0,
                    // округление в меньшую сторону до двух чисел после точки
                    2, MidpointRounding.ToZero);

                embed.AddField(
                    IzumiReplyMessage.UserMasteryFieldName.Parse(
                        emotes.GetEmoteOrBlank(mastery.ToString()), userMasteryAmount, mastery.Localize()),
                    mastery.Description());
            }

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
