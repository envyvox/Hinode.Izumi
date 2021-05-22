using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteSendCommand
{
    [InjectableService]
    public class FamilyInviteSendCommand : IFamilyInviteSendCommand
    {
        private readonly IMediator _mediator;

        public FamilyInviteSendCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string username)
        {
            // получаем пользователя в семье
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));

            // проверяем что пользователь является владельцем семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // получаем пользователя цель
                var tUser = await _mediator.Send(new GetUserByNamePatternQuery(username));
                // получаем информацию о том, в какой семье состоит цель
                var tUserHasFamily = await _mediator.Send(new CheckUserHasFamilyQuery(tUser.Id));

                // проверяем что цель не состоит в семье
                if (tUserHasFamily)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyNotNull.Parse()));
                }
                else
                {
                    // получаем приглашение в семью для этой цели
                    var invite = await _mediator.Send(new GetFamilyInviteByParamsQuery(userFamily.FamilyId, tUser.Id));

                    // проверяем не отправляла ли уже семья приглашение этому пользователю
                    if (invite is not null)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.FamilyInviteSendAlready.Parse()));
                    }
                    else
                    {
                        // получаем иконки из базы
                        var emotes = await _mediator.Send(new GetEmotesQuery());
                        // получаем семью
                        var family = await _mediator.Send(new GetFamilyByIdQuery(userFamily.FamilyId));

                        // создаем приглашение
                        await _mediator.Send(new CreateFamilyInviteCommand(userFamily.FamilyId, tUser.Id));

                        var embed = new EmbedBuilder()
                            // подтверждаем что приглашение успешно создано
                            .WithDescription(IzumiReplyMessage.FamilyInviteSendSuccess.Parse(
                                emotes.GetEmoteOrBlank(tUser.Title.Emote()), tUser.Title.Localize(),
                                tUser.Name));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                        var notify = new EmbedBuilder()
                            // оповещаем цель о том, что ее пригласили в семью
                            .WithDescription(IzumiReplyMessage.FamilyInviteSendSuccessNotify.Parse(family.Name));

                        await _mediator.Send(new SendEmbedToUserCommand(
                            await _mediator.Send(new GetDiscordSocketUserQuery(tUser.Id)), notify));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
