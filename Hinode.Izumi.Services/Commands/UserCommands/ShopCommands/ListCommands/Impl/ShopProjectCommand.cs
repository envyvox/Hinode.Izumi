using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProjectService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopProjectCommand : IShopProjectCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IProjectService _projectService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly IBuildingService _buildingService;

        public ShopProjectCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IProjectService projectService, ILocalizationService local, IImageService imageService,
            IBuildingService buildingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _projectService = projectService;
            _local = local;
            _imageService = imageService;
            _buildingService = buildingService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем все чертежи
            var projects = await _projectService.GetAllProjects();

            var embed = new EmbedBuilder()
                // изображение магазина чертежей
                .WithImageUrl(await _imageService.GetImageUrl(Image.ShopProject))
                // рассказываем как купить чертеж
                .WithDescription(
                    IzumiReplyMessage.ShopProjectDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого чертежа создаем embed field
            foreach (var project in projects)
            {
                // проверяем наличие чертежа у пользователя
                var hasProject = await _projectService.CheckUserHasProject((long) context.User.Id, project.Id);
                // если у пользователя уже есть этот чертеж - игнорируем
                if (hasProject) return;

                embed.AddField(
                    $"{emotes.GetEmoteOrBlank("List")} `{project.Id}` {emotes.GetEmoteOrBlank("Project")} {project.Name}",
                    $"Стоимость: {emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {project.Price} {_local.Localize(Currency.Ien.ToString(), project.Price)}");
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
