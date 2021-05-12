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
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Humanizer;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingItemInfoCommand
{
    [InjectableService]
    public class CraftingItemInfoCommand : ICraftingItemInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICraftingService _craftingService;
        private readonly ILocalizationService _local;
        private readonly IImageService _imageService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;

        public CraftingItemInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICraftingService craftingService, ILocalizationService local, IImageService imageService,
            ICalculationService calc, IIngredientService ingredientService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _craftingService = craftingService;
            _local = local;
            _imageService = imageService;
            _calc = calc;
            _ingredientService = ingredientService;
        }

        public async Task Execute(SocketCommandContext context, long craftingId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем изготавливаемый предмет
            var crafting = await _craftingService.GetCrafting(craftingId);

            // получаем локализированную строку с ингредиентами изготавливаемого предмета
            var ingredients = await _ingredientService.DisplayCraftingIngredients(crafting.Id);
            // получаем стоимость изготовления
            var craftingPrice = await _calc.CraftingPrice(
                // получаем себестоимость изготавливаемого предмета
                await _ingredientService.GetCraftingCostPrice(crafting.Id));

            var embed = new EmbedBuilder()
                .WithTitle($"`{crafting.Id}` {emotes.GetEmoteOrBlank(crafting.Name)} {_local.Localize(crafting.Name)}")
                // изображение изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
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

            await _discordEmbedService.SendEmbed(context.User, embed);
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
