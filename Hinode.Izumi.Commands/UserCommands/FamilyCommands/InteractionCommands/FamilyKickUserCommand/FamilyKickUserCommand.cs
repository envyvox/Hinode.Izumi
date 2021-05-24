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

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyKickUserCommand
{
    [InjectableService]
    public class FamilyKickUserCommand : IFamilyKickUserCommand
    {
        private readonly IMediator _mediator;

        public FamilyKickUserCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(SocketCommandContext context, string username)
        {
            // получаем пользователя в семье
            var userFamily = await _mediator.Send(new GetUserFamilyQuery((long) context.User.Id));

            // проверяем что пользователь является главой семьи
            if (userFamily.Status != UserInFamilyStatus.Head)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.UserFamilyStatusRequireHead.Parse()));
            }
            else
            {
                // получаем пользователя цель
                var tUser = await _mediator.Send(new GetUserByNamePatternQuery(username));

                // проверяем что пользователь не является целью
                if (tUser.Id == (long) context.User.Id)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.FamilyKickYourself.Parse()));
                }
                else
                {
                    // получаем информацию о том, в какой семье постоит цель
                    var tUserFamily = await _mediator.Send(new GetUserFamilyQuery(tUser.Id));

                    // проверяем что цель находится в одной семье с пользователем
                    if (tUserFamily.FamilyId != userFamily.FamilyId)
                    {
                        await Task.FromException(new Exception(IzumiReplyMessage.UserNotInYourFamily.Parse()));
                    }
                    else
                    {
                        // исключаем цель из семьи
                        await _mediator.Send(new RemoveUserFromFamilyCommand(tUser.Id));

                        // получаем иконки из базы
                        var emotes = await _mediator.Send(new GetEmotesQuery());
                        var embed = new EmbedBuilder()
                            // подтверждаем успешное исключение цели из семьи
                            .WithDescription(IzumiReplyMessage.FamilyKickUserSuccess.Parse(
                                emotes.GetEmoteOrBlank(tUser.Title.Emote()),
                                tUser.Title.Localize(), tUser.Name));

                        await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

                        // получаем семью пользователя
                        var family = await _mediator.Send(new GetFamilyByIdQuery(tUserFamily.FamilyId));
                        var notify = new EmbedBuilder()
                            // оповещаем цель о том, что ее исключили
                            .WithDescription(IzumiReplyMessage.FamilyKickUserSuccessNotify.Parse(family.Name));

                        await _mediator.Send(new SendEmbedToUserCommand(
                            await _mediator.Send(new GetDiscordSocketUserQuery(tUser.Id)), notify));
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}
