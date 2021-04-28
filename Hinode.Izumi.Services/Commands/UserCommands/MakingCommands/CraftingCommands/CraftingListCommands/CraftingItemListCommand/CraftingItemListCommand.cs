using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.
    CraftingItemListCommand
{
    [InjectableService]
    public class CraftingItemListCommand : ICraftingItemListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly ICraftingService _craftingService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;

        public CraftingItemListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            ICraftingService craftingService, IImageService imageService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _craftingService = craftingService;
            _imageService = imageService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем изготавливаемые предметы
            var craftings = await _craftingService.GetAllCraftings();

            var embed = new EmbedBuilder()
                // изображение изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
                // рассказываем как изготовить алкоголь
                .WithDescription(
                    IzumiReplyMessage.CraftingItemListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого изготавливаемого предмета создаем embed field
            foreach (var crafting in craftings)
            {
                embed.AddField(
                    // выводим название изготавливаемого предмета
                    IzumiReplyMessage.CraftingListFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), crafting.Id, emotes.GetEmoteOrBlank(crafting.Name),
                        _local.Localize(crafting.Name, 5), crafting.Location.Localize(true)),
                    $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
