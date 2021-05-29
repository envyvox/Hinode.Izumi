using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CollectionService.Commands;
using Hinode.Izumi.Services.GameServices.CollectionService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.BuyCommands.Impl
{
    [InjectableService]
    public class ShopBuyEventCommand : IShopBuyEventCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public ShopBuyEventCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long itemId, string namePattern = null)
        {
            var currentEvent = (Event) await _mediator.Send(new GetPropertyValueQuery(Property.CurrentEvent));

            if (currentEvent is Event.None)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopEventRequireEvent.Parse()));
            }
            else
            {
                _emotes = await _mediator.Send(new GetEmotesQuery());

                var embed = currentEvent switch
                {
                    Event.June => await TryBuyFromJuneEventShop(
                        (long) context.User.Id, (BambooToy) itemId, namePattern),
                    _ => new EmbedBuilder().WithDescription(IzumiReplyMessage.ShopEventEmpty.Parse())
                };

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            }
        }

        private async Task<EmbedBuilder> TryBuyFromJuneEventShop(long buyerId, BambooToy bambooToy, string namePattern)
        {
            var bamboo = await _mediator.Send(new GetGatheringByNameQuery("Bamboo"));
            var userBamboo = await _mediator.Send(new GetUserGatheringQuery(buyerId, bamboo.Id));
            var bambooToyPrice = await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneBambooToyPrice));
            var embed = new EmbedBuilder();

            if (userBamboo.Amount < bambooToyPrice)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.ShopBuyNoCurrency.Parse(
                    _emotes.GetEmoteOrBlank("Bamboo"), _local.Localize("Bamboo", 2))));
            }
            else
            {
                var user = namePattern is null
                    ? await _mediator.Send(new GetUserByIdQuery(buyerId))
                    : await _mediator.Send(new GetUserByNamePatternQuery(namePattern));
                var userHasToy = await _mediator.Send(new CheckUserHasCollectionQuery(
                    user.Id, CollectionCategory.Event, bambooToy.GetHashCode()));

                if (userHasToy)
                {
                    await Task.FromException(new Exception(namePattern is null
                        ? IzumiReplyMessage.ShopEventJuneBuyYouHaveToy.Parse(
                            _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString()))
                        : IzumiReplyMessage.ShopEventJuneBuyUserHasToy.Parse(
                            _emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name,
                            _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString()))));
                }
                else
                {
                    await _mediator.Send(new AddCollectionToUserCommand(
                        user.Id, CollectionCategory.Event, bambooToy.GetHashCode(), Event.June));
                    await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                        buyerId, InventoryCategory.Gathering, bamboo.Id, bambooToyPrice));


                    embed.WithDescription(namePattern is null
                        ? IzumiReplyMessage.ShopEventJuneBuySuccessYourself.Parse(
                            _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString()),
                            _emotes.GetEmoteOrBlank("Bamboo"), bambooToyPrice,
                            _local.Localize("Bamboo", bambooToyPrice))
                        : IzumiReplyMessage.ShopEventJuneBuySuccessForUser.Parse(
                            _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString()),
                            _emotes.GetEmoteOrBlank("Bamboo"), bambooToyPrice,
                            _local.Localize("Bamboo", bambooToyPrice),
                            _emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name));

                    if (namePattern is not null)
                    {
                        var buyer = await _mediator.Send(new GetUserByIdQuery(buyerId));
                        var embedNotify = new EmbedBuilder()
                            .WithDescription(IzumiReplyMessage.ShopEventJuneBuySuccessNotify.Parse(
                                _emotes.GetEmoteOrBlank(buyer.Title.Emote()), buyer.Title.Localize(), buyer.Name,
                                _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString())));

                        await _mediator.Send(new SendEmbedToUserCommand(
                            await _mediator.Send(new GetDiscordSocketUserQuery(user.Id)), embedNotify));
                    }
                }
            }

            return embed;
        }
    }
}
