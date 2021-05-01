using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProjectService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyProjectCommand : IShopBuyProjectCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IProjectService _projectService;
        private readonly IInventoryService _inventoryService;
        private readonly IImageService _imageService;

        public ShopBuyProjectCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ILocalizationService local, IProjectService projectService, IInventoryService inventoryService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _local = local;
            _projectService = projectService;
            _inventoryService = inventoryService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context, long projectId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем чертеж
            var project = await _projectService.GetProject(projectId);
            // проверяем есть ли у пользователя уже этот чертеж
            var hasProject = await _projectService.CheckUserHasProject((long) context.User.Id, project.Id);

            if (hasProject)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyProjectAlreadyHaveProject.Parse(
                    emotes.GetEmoteOrBlank("Project"), project.Name)));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _inventoryService.GetUserCurrency((long) context.User.Id, Currency.Ien);

                // проверяем хватает ли пользователю на оплату чертежа
                if (userCurrency.Amount < project.Price)
                {
                    await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                        emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                        _local.Localize(Currency.Ien.ToString(), 5))));
                }
                else
                {
                    // отнимаем у пользователя деньги на оплату чертежа
                    await _inventoryService.RemoveItemFromUser(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), project.Price);
                    // добавляем пользователю чертеж
                    await _projectService.AddProjectToUser((long) context.User.Id, project.Id);

                    var embed = new EmbedBuilder()
                        // баннер магазина чертежей
                        .WithImageUrl(await _imageService.GetImageUrl(Image.ShopProject))
                        // подверждаем успешную покупку чертежа
                        .WithDescription(IzumiReplyMessage.ShopBuyProjectSuccess.Parse(
                            emotes.GetEmoteOrBlank("Project"), project.Name,
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), project.Price,
                            _local.Localize(Currency.Ien.ToString(), project.Price)));

                    await _discordEmbedService.SendEmbed(context.User, embed);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
