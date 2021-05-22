using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemInfoCommand
{
    [InjectableService]
    public class CraftingItemInfoCommand : ICraftingItemInfoCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingItemInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long craftingId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем изготавливаемый предмет
            var crafting = await _mediator.Send(new GetCraftingQuery(craftingId));

            // получаем локализированную строку с ингредиентами изготавливаемого предмета
            var ingredients = await _mediator.Send(new DisplayCraftingIngredientQuery(crafting.Id));
            // получаем стоимость изготовления
            var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(
                // получаем себестоимость изготавливаемого предмета
                await _mediator.Send(new GetCraftingCostPriceQuery(crafting.Id))));

            var embed = new EmbedBuilder()
                .WithTitle($"`{crafting.Id}` {emotes.GetEmoteOrBlank(crafting.Name)} {_local.Localize(crafting.Name)}")
                // изображение изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // рассказываем как изготовить изготавливаемый предмет
                .WithDescription(
                    IzumiReplyMessage.CraftingItemInfoDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // локация
                .AddField(IzumiReplyMessage.LocationFieldName.Parse(),
                    crafting.Location.Localize())
                // необходимые ингредиенты
                .AddField(IzumiReplyMessage.IngredientsFieldName.Parse(), ingredients)
                // стоимость изготовления
                .AddField(IzumiReplyMessage.CraftingPriceFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {craftingPrice} {_local.Localize(Currency.Ien.ToString(), craftingPrice)}",
                    true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    crafting.Time.Seconds().Humanize(2, new CultureInfo("ru-RU")), true);

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        public async Task Execute(SocketCommandContext context, string itemNamePattern)
        {
            // получаем локализацию предмета
            var itemLocalization = await _local.GetLocalizationByLocalizedWord(
                LocalizationCategory.Crafting, itemNamePattern);
            // и используем основной метод уже зная id предмета
            await Execute(context, itemLocalization.ItemId);
        }
    }
}
