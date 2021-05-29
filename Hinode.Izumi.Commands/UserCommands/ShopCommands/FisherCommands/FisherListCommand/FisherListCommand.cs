using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.FisherCommands.FisherListCommand
{
    [InjectableService]
    public class FisherListCommand : IFisherListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public FisherListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущий сезон в мире
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем всю рыбу текущего сезона
            var fish = await _mediator.Send(new GetAllFishBySeasonQuery(season));
            // разбиваем ее по редкости
            var commonFish = fish.Where(x => x.Rarity == FishRarity.Common);
            var rareFish = fish.Where(x => x.Rarity == FishRarity.Rare);
            var epicFish = fish.Where(x => x.Rarity == FishRarity.Epic);
            var mythicalFish = fish.Where(x => x.Rarity == FishRarity.Mythical);
            var legendaryFish = fish.Where(x => x.Rarity == FishRarity.Legendary);

            var embed = new EmbedBuilder()
                // рассказываем как продавать рыбу
                .WithDescription(
                    IzumiReplyMessage.FisherShopDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина рыбака
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopFisher)))
                // заполняем embed field рыбой и ее стоимостью
                .AddField(FishRarity.Common.Localize(), Display(commonFish))
                .AddField(FishRarity.Rare.Localize(), Display(rareFish))
                .AddField(FishRarity.Epic.Localize(), Display(epicFish))
                .AddField(FishRarity.Mythical.Localize(), Display(mythicalFish))
                .AddField(FishRarity.Legendary.Localize(), Display(legendaryFish));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        /// <summary>
        /// Возвращает локализированную строку рыбы (иконка, название, стоимость).
        /// </summary>
        /// <param name="model">Массив рыбы.</param>
        /// <returns>Локализированную строку рыбы.</returns>
        private string Display(IEnumerable<FishRecord> model)
        {
            return model
                .Aggregate(string.Empty,
                    (current, fish) =>
                        current + IzumiReplyMessage.FisherShopFishDesc.Parse(
                            _emotes.GetEmoteOrBlank("List"), fish.Id, _emotes.GetEmoteOrBlank(fish.Name),
                            _local.Localize(fish.Name), _emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                            fish.Price, _local.Localize(Currency.Ien.ToString(), fish.Price)));
        }
    }
}
