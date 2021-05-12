using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingListCommands.
    CraftingAlcoholListCommand;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.ImageService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MakingCommands.CraftingCommands.CraftingAlcoholListCommand
{
    [InjectableService]
    public class CraftingAlcoholListCommand : ICraftingAlcoholListCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IAlcoholService _alcoholService;
        private readonly IImageService _imageService;
        private readonly ILocalizationService _local;

        public CraftingAlcoholListCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IAlcoholService alcoholService, IImageService imageService, ILocalizationService local)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _alcoholService = alcoholService;
            _imageService = imageService;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем алкоголь
            var alcohols = await _alcoholService.GetAllAlcohol();

            var embed = new EmbedBuilder()
                // изображение изготовления
                .WithImageUrl(await _imageService.GetImageUrl(Image.Crafting))
                // рассказываем как изготовить алкоголь
                .WithDescription(
                    IzumiReplyMessage.CraftingAlcoholListDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}");

            // для каждого алкоголя создаем embed field
            foreach (var alcohol in alcohols)
            {
                embed.AddField(
                    // выводим название изготавливаемого алкоголя
                    IzumiReplyMessage.CraftingListFieldName.Parse(
                        emotes.GetEmoteOrBlank("List"), alcohol.Id, emotes.GetEmoteOrBlank(alcohol.Name),
                        _local.Localize(alcohol.Name, 5), Location.Village.Localize(true)),
                    $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
