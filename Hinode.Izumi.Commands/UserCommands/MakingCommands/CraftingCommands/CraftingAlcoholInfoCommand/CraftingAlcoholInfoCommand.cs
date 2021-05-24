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
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.IngredientService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholInfoCommand
{
    [InjectableService]
    public class CraftingAlcoholInfoCommand : ICraftingAlcoholInfoCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CraftingAlcoholInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long alcoholId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем алкоголь
            var alcohol = await _mediator.Send(new GetAlcoholQuery(alcoholId));

            // получаем локализированную строку с ингредиентами алкоголя
            var ingredients = await _mediator.Send(new DisplayAlcoholIngredientsQuery(alcohol.Id));
            // получаем стоимость изготовления
            var craftingPrice = await _mediator.Send(new GetCraftingPriceQuery(
                await _mediator.Send(new GetAlcoholCostPriceQuery(alcohol.Id))));

            var embed = new EmbedBuilder()
                .WithTitle($"`{alcohol.Id}` {emotes.GetEmoteOrBlank(alcohol.Name)} {_local.Localize(alcohol.Name)}")
                // изображение изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // рассказываем как изготовить алкоголь
                .WithDescription(
                    IzumiReplyMessage.CraftingAlcoholInfoDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // локация
                .AddField(IzumiReplyMessage.LocationFieldName.Parse(),
                    Location.Village.Localize())
                // необходимые ингредиенты
                .AddField(IzumiReplyMessage.IngredientsFieldName.Parse(), ingredients)
                // стоимость изготовления
                .AddField(IzumiReplyMessage.CraftingPriceFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {craftingPrice} {_local.Localize(Currency.Ien.ToString(), craftingPrice)}",
                    true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    alcohol.Time.Seconds().Humanize(2, new CultureInfo("ru-RU")), true);

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        public async Task Execute(SocketCommandContext context, string alcoholNamePattern)
        {
            // получаем локализацию алкоголя
            var alcoholLocalization = await _local.GetLocalizationByLocalizedWord(
                LocalizationCategory.Alcohol, alcoholNamePattern);
            // и используем основной метод уже зная id алкоголя
            await Execute(context, alcoholLocalization.ItemId);
        }
    }
}
