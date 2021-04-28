using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingInfoCommands.
    CraftingAlcoholInfoCommand
{
    [InjectableService]
    public class CraftingAlcoholInfoCommand : ICraftingAlcoholInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IIngredientService _ingredientService;
        private readonly ILocalizationService _local;
        private readonly IAlcoholService _alcoholService;
        private readonly IImageService _imageService;
        private readonly ICalculationService _calc;

        public CraftingAlcoholInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IIngredientService ingredientService, ILocalizationService local, IAlcoholService alcoholService,
            IImageService imageService, ICalculationService calc)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _ingredientService = ingredientService;
            _local = local;
            _alcoholService = alcoholService;
            _imageService = imageService;
            _calc = calc;
        }

        public async Task Execute(SocketCommandContext context, long alcoholId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем алкоголь
            var alcohol = await _alcoholService.GetAlcohol(alcoholId);

            // получаем локализированную строку с ингредиентами алкоголя
            var ingredients = await _ingredientService.DisplayAlcoholIngredients(alcohol.Id);
            // получаем стоимость изготовления
            var craftingPrice = await _calc.CraftingPrice(
                await _ingredientService.GetAlcoholCostPrice(alcohol.Id));

            var embed = new EmbedBuilder()
                .WithTitle($"`{alcohol.Id}` {emotes.GetEmoteOrBlank(alcohol.Name)} {_local.Localize(alcohol.Name)}")
                // изображение изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
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
                    $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {craftingPrice} {_local.Localize(Currency.Ien.ToString(), craftingPrice)}", true)
                // длительность
                .AddField(IzumiReplyMessage.TimeFieldName.Parse(),
                    alcohol.Time.Seconds().Humanize(2, new CultureInfo("ru-RU")), true);

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
