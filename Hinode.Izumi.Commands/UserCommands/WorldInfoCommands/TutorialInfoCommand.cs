using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.GameServices.TutorialService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.WorldInfoCommands
{
    [CommandCategory(CommandCategory.Training)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class TutorialInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;

        public TutorialInfoCommand(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Command("обучение"), Alias("tutorial")]
        [Summary("Начать обучение, или если оно уже начато - посмотреть информацию о текущем шаге обучения")]
        public async Task TutorialInfoTask()
        {
            // получаем текущий шаг обучения пользователя
            var userTrainingStep = await _mediator.Send(new GetUserTutorialStepQuery((long) Context.User.Id));
            var step = userTrainingStep;

            // если у пользователя нет текущего шага обучения - устанавливаем ему первый шаг
            if (userTrainingStep == TutorialStep.None)
            {
                await _mediator.Send(new UpdateUserTutorialStepCommand(
                    (long) Context.User.Id, TutorialStep.CheckProfile));
                step = TutorialStep.CheckProfile;
            }

            var embed = new EmbedBuilder()
                // изображение смайлика
                .WithThumbnailUrl(await _mediator.Send(new GetImageUrlQuery(Image.Tutorial)))
                // название шага обучения
                .WithAuthor(step.Name())
                // описание шага обучения
                .WithDescription(step.Description());

            await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));
            await Task.CompletedTask;
        }
    }
}
