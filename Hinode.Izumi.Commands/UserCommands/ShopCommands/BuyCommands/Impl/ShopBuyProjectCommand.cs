using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ProjectService.Commands;
using Hinode.Izumi.Services.GameServices.ProjectService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyProjectCommand : IShopBuyProjectCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopBuyProjectCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long projectId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем чертеж
            var project = await _mediator.Send(new GetProjectQuery(projectId));
            // проверяем есть ли у пользователя уже этот чертеж
            var hasProject = await _mediator.Send(new CheckUserHasProjectQuery((long) context.User.Id, project.Id));

            if (hasProject)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyProjectAlreadyHaveProject.Parse(
                    emotes.GetEmoteOrBlank("Project"), project.Name)));
            }
            else
            {
                // получаем валюту пользователя
                var userCurrency = await _mediator.Send(new GetUserCurrencyQuery((long) context.User.Id, Currency.Ien));

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
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        (long) context.User.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), project.Price));
                    // добавляем пользователю чертеж
                    await _mediator.Send(new AddProjectToUserCommand((long) context.User.Id, project.Id));

                    var embed = new EmbedBuilder()
                        // баннер магазина чертежей
                        .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopProject)))
                        // подверждаем успешную покупку чертежа
                        .WithDescription(IzumiReplyMessage.ShopBuyProjectSuccess.Parse(
                            emotes.GetEmoteOrBlank("Project"), project.Name,
                            emotes.GetEmoteOrBlank(Currency.Ien.ToString()), project.Price,
                            _local.Localize(Currency.Ien.ToString(), project.Price)));

                    await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
                    await Task.CompletedTask;
                }
            }
        }
    }
}
