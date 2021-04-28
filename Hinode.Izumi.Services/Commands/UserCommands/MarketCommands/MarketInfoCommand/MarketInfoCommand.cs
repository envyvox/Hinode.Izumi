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
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.Commands.UserCommands.MarketCommands.MarketInfoCommand
{
    [InjectableService]
    public class MarketInfoCommand : IMarketInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IImageService _imageService;
        private readonly ITrainingService _trainingService;

        public MarketInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IImageService imageService, ITrainingService trainingService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _imageService = imageService;
            _trainingService = trainingService;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            var embed = new EmbedBuilder()
                // баннер рынка
                .WithImageUrl(await _imageService.GetImageUrl(Image.LocationCapitalMarket))
                .WithDescription(IzumiReplyMessage.MarketInfoDesc.Parse())
                // рассказываем как покупать на рынке
                .AddField(IzumiReplyMessage.MarketInfoBuyFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoBuyFieldDesc.Parse())
                // рассказываем как продавать на рынке
                .AddField(IzumiReplyMessage.MarketInfoSellFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoSellFieldDesc.Parse())
                // рассказываем как управлять своими заявками на рынке
                .AddField(IzumiReplyMessage.MarketInfoRequestFieldName.Parse(),
                    IzumiReplyMessage.MarketInfoRequestFieldDesc.Parse())
                // выводим список разрешенных групп товаров на рынке
                .AddField(IzumiReplyMessage.MarketInfoGroupsFieldName.Parse(),
                    Enum.GetValues(typeof(MarketCategory))
                        .Cast<MarketCategory>()
                        .Aggregate(string.Empty, (current, category) =>
                            current +
                            $"{emotes.GetEmoteOrBlank("List")} `{category.GetHashCode()}` {category.Localize()}\n"));

            await _discordEmbedService.SendEmbed(context.User, embed);
            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _trainingService.CheckStep((long) context.User.Id, TrainingStep.CheckCapitalMarket);
            await Task.CompletedTask;
        }
    }
}
