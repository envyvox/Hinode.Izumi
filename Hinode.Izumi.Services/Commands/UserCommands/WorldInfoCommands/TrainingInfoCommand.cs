using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.WorldInfoCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class TrainingInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly ITrainingService _trainingService;
        private readonly IImageService _imageService;

        public TrainingInfoCommand(IDiscordEmbedService discordEmbedService, ITrainingService trainingService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _trainingService = trainingService;
            _imageService = imageService;
        }

        [Command("обучение"), Alias("training")]
        public async Task TrainingInfoTask()
        {
            // получаем текущий шаг обучения пользователя
            var userTrainingStep = await _trainingService.GetUserTrainingStep((long) Context.User.Id);
            var step = userTrainingStep;

            // если у пользователя нет текущего шага обучения - устанавливаем ему первый шаг
            if (userTrainingStep == TrainingStep.None)
            {
                await _trainingService.UpdateUserTrainingStep((long) Context.User.Id, TrainingStep.CheckProfile);
                step = TrainingStep.CheckProfile;
            }

            var embed = new EmbedBuilder()
                // изображение смайлика
                .WithThumbnailUrl(await _imageService.GetImageUrl(Image.Training))
                // название шага обучения
                .WithAuthor(step.Name())
                // описание шага обучения
                .WithDescription(step.Description());

            await _discordEmbedService.SendEmbed(Context.User, embed);
            await Task.CompletedTask;
        }
    }
}
