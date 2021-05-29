using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.ShopCommands.ListCommands.Impl
{
    [InjectableService]
    public class ShopEventCommand : IShopEventCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public ShopEventCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context)
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
                    Event.June => await BuildJuneEventShop(),
                    _ => new EmbedBuilder().WithDescription(IzumiReplyMessage.ShopEventEmpty.Parse())
                };

                await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            }
        }

        private async Task<EmbedBuilder> BuildJuneEventShop()
        {
            var bambooToyPrice = await _mediator.Send(new GetPropertyValueQuery(Property.EventJuneBambooToyPrice));
            var embed = new EmbedBuilder()
                .WithAuthor(IzumiReplyMessage.ShopEventJuneAuthor.Parse())
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Summer)))
                .WithDescription(
                    IzumiReplyMessage.ShopEventJuneDesc.Parse() +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}");

            foreach (var bambooToy in Enum.GetValues(typeof(BambooToy))
                .Cast<BambooToy>())
            {
                embed.AddField(
                    IzumiReplyMessage.ShopEventJuneToyFieldName.Parse(
                        _emotes.GetEmoteOrBlank("List"), bambooToy.GetHashCode(),
                        _emotes.GetEmoteOrBlank(bambooToy.Emote()), _local.Localize(bambooToy.ToString())),
                    IzumiReplyMessage.ShopEventJuneToyFieldDesc.Parse(
                        _emotes.GetEmoteOrBlank("Bamboo"), bambooToyPrice, _local.Localize("Bamboo", bambooToyPrice)));
            }

            return embed;
        }
    }
}
