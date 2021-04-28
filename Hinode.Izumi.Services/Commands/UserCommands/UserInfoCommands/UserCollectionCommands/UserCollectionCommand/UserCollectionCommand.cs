using System;
using System.Linq;
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
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserCollectionCommands.UserCollectionCommand
{
    [InjectableService]
    public class UserCollectionCommand : IUserCollectionCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;

        public UserCollectionCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем все иконки из базы
            var emotes = await _emoteService.GetEmotes();
            var embed = new EmbedBuilder()
                // баннер коллекции
                .WithImageUrl(await _imageService.GetImageUrl(Image.Collection))
                // рассказываем как просматривать коллекцию по категориям
                .WithDescription(IzumiReplyMessage.UserCollectionDesc.Parse())
                // выводим список категорий коллекции с номером и названием
                .AddField(IzumiReplyMessage.UserCollectionFieldName.Parse(),
                    Enum.GetValues(typeof(CollectionCategory))
                        .Cast<CollectionCategory>()
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }
    }
}
