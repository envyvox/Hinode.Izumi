using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.SeedService.Queries;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using Humanizer;
using Humanizer.Localisation;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopSeedCommand : IShopSeedCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public ShopSeedCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем текущий сезон
            var season = (Season) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentSeason));
            // получаем семена текущего сезона
            var seeds = await _mediator.Send(new GetAllSeedsWithSeasonQuery(season));
            // получаем мастерство торговли пользователя
            var userMastery = await _mediator.Send(new GetUserMasteryQuery((long) context.User.Id, Mastery.Trading));

            var embed = new EmbedBuilder()
                // рассказываем как покупать семена
                .WithDescription(
                    IzumiReplyMessage.CapitalSeedShopDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // баннер магазина семян
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.ShopSeed)));

            // для каждого семени создаем embed field
            foreach (var seed in seeds)
            {
                // получаем урожай из этого семени
                var crop = await _mediator.Send(new GetCropBySeedIdQuery(seed.Id));
                // определяем стоимость семени
                var seedPrice = await _mediator.Send(new GetSeedPriceWithDiscountQuery(
                    // округяем мастерство пользователя
                    (long) Math.Floor(userMastery.Amount), seed.Price));

                // заполняем информацию о семени
                var seedDesc = IzumiReplyMessage.CapitalSeedShopSeedDesc.Parse(
                    seed.Growth.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day),
                    emotes.GetEmoteOrBlank(crop.Name), _local.Localize(crop.Name),
                    emotes.GetEmoteOrBlank(Currency.Ien.ToString()), crop.Price,
                    _local.Localize(Currency.Ien.ToString(), crop.Price));

                // если семя имеет возможность давать несколько плодов из одного семени - добавляем строку об этом
                if (seed.Multiply)
                    seedDesc += IzumiReplyMessage.CapitalSeedShopSeedMultiple.Parse(emotes.GetEmoteOrBlank("List"));
                // если семя имеет возможность повторного роста - добавляем строку об этом
                if (seed.ReGrowth > 0)
                    seedDesc += IzumiReplyMessage.CapitalSeedShopSeedReGrowth.Parse(emotes.GetEmoteOrBlank("List"),
                        seed.ReGrowth.Days().Humanize(1, new CultureInfo("ru-RU"), TimeUnit.Day));

                // выводим информацию о семени
                embed.AddField(IzumiReplyMessage.CapitalSeedShopSeedFieldName.Parse(
                    emotes.GetEmoteOrBlank("List"), seed.Id, emotes.GetEmoteOrBlank(seed.Name),
                    _local.Localize(seed.Name), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    // если цена по-умолчанию отличается от финальной стоимости, выводим строку с отображением скидки
                    seed.Price != seedPrice
                        ? $"~~{seed.Price}~~ {seedPrice} {_local.Localize(Currency.Ien.ToString(), seedPrice)}"
                        : $"{seed.Price} {_local.Localize(Currency.Ien.ToString(), seed.Price)}"
                ), seedDesc + $"{emotes.GetEmoteOrBlank("Blank")}");
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(
                (long) context.User.Id, TutorialStep.CheckCapitalSeedShop));

            await Task.CompletedTask;
        }
    }
}
