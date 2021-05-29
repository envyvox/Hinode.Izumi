using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserInventoryCommands.UserInventoryCropCommand
{
    [InjectableService]
    public class UserInventoryCropCommand : IUserInventoryCropCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public UserInventoryCropCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем урожай пользователя
            var userCrops = await _mediator.Send(new GetUserCropsQuery((long) context.User.Id));
            // разбиваем его сезоны
            var springCrops = userCrops.Where(x => x.Season == Season.Spring);
            var summerCrops = userCrops.Where(x => x.Season == Season.Summer);
            var autumnCrops = userCrops.Where(x => x.Season == Season.Autumn);

            var springCropsString = Display(springCrops);
            var summerCropsString = Display(summerCrops);
            var autumnCropsString = Display(autumnCrops);

            var embed = new EmbedBuilder()
                // баннер инвентаря
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Inventory)))
                .WithDescription(IzumiReplyMessage.UserCropsDesc.Parse())
                // весенний урожай
                .AddField(IzumiReplyMessage.UserCropsSpringFieldName.Parse(),
                    springCropsString.Length > 0
                        ? springCropsString.Remove(springCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // летний урожай
                .AddField(IzumiReplyMessage.UserCropsSummerFieldName.Parse(),
                    summerCropsString.Length > 0
                        ? summerCropsString.Remove(summerCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse())
                // осенний урожай
                .AddField(IzumiReplyMessage.UserCropsAutumnFieldName.Parse(),
                    autumnCropsString.Length > 0
                        ? autumnCropsString.Remove(autumnCropsString.Length - 2)
                        : IzumiReplyMessage.InventoryNull.Parse());

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированное отображение урожая (иконка, количество, название) через запятую.
        /// </summary>
        /// <param name="cropInUser">Массив урожая у пользователя.</param>
        /// <returns>Локализированная строка урожая.</returns>
        private string Display(IEnumerable<UserCropRecord> cropInUser)
        {
            return cropInUser
                .Aggregate(string.Empty,
                    (current, crop) =>
                        current + (crop.Amount > 0
                            ? $"{_emotes.GetEmoteOrBlank(crop.Name)} {crop.Amount} {_local.Localize(crop.Name, crop.Amount)}, "
                            : ""));
        }
    }
}
