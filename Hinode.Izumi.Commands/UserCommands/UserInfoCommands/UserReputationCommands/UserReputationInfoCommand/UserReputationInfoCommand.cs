using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Records;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationInfoCommand
{
    [InjectableService]
    public class UserReputationInfoCommand : IUserReputationInfoCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public UserReputationInfoCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, Reputation reputation)
        {
            _emotes = await _mediator.Send(new GetEmotesQuery());
            var userReputation = await _mediator.Send(new GetUserReputationQuery((long) context.User.Id, reputation));

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"У вас {_emotes.GetEmoteOrBlank(reputation.Emote(userReputation.Amount))} {userReputation.Amount} репутации в **{reputation.Location().Localize(true)}**." +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                .AddField("Награды за получение репутации", await DisplayRewards(userReputation));

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));
            await Task.CompletedTask;
        }

        private async Task<string> DisplayRewards(UserReputationRecord userReputation)
        {
            var rewardString = string.Empty;
            Box box;
            long boxAmount;
            long foodId;
            long pearlAmount;
            long cardId;
            Title title;

            switch (userReputation.Reputation)
            {
                case Reputation.Capital:

                    box = Box.Capital;
                    boxAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCapitalBoxAmount));
                    foodId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCapitalFoodId));
                    pearlAmount =
                        await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCapitalPearlAmount));
                    cardId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCapitalCardId));
                    title = (Title) await _mediator.Send(
                        new GetPropertyValueQuery(Property.ReputationCapitalTitleNumber));

                    break;
                case Reputation.Garden:

                    box = Box.Garden;
                    boxAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationGardenBoxAmount));
                    foodId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationGardenFoodId));
                    pearlAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationGardenPearlAmount));
                    cardId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationGardenCardId));
                    title = (Title) await _mediator.Send(
                        new GetPropertyValueQuery(Property.ReputationGardenTitleNumber));

                    break;
                case Reputation.Seaport:

                    box = Box.Seaport;
                    boxAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationSeaportBoxAmount));
                    foodId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationSeaportFoodId));
                    pearlAmount =
                        await _mediator.Send(new GetPropertyValueQuery(Property.ReputationSeaportPearlAmount));
                    cardId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationSeaportCardId));
                    title = (Title) await _mediator.Send(
                        new GetPropertyValueQuery(Property.ReputationSeaportTitleNumber));

                    break;
                case Reputation.Castle:

                    box = Box.Castle;
                    boxAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCastleBoxAmount));
                    foodId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCastleFoodId));
                    pearlAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCastlePearlAmount));
                    cardId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationCastleCardId));
                    title = (Title) await _mediator.Send(
                        new GetPropertyValueQuery(Property.ReputationCastleTitleNumber));

                    break;
                case Reputation.Village:

                    box = Box.Village;
                    boxAmount = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationVillageBoxAmount));
                    foodId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationVillageFoodId));
                    pearlAmount =
                        await _mediator.Send(new GetPropertyValueQuery(Property.ReputationVillagePearlAmount));
                    cardId = await _mediator.Send(new GetPropertyValueQuery(Property.ReputationVillageCardId));
                    title = (Title) await _mediator.Send(
                        new GetPropertyValueQuery(Property.ReputationVillageTitleNumber));

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var food = await _mediator.Send(new GetFoodQuery(foodId));
            var card = await _mediator.Send(new GetCardQuery(cardId));

            rewardString +=
                $"{_emotes.GetEmoteOrBlank(userReputation.Amount >= 500 ? "Checkmark" : "List")} За {_emotes.GetEmoteOrBlank(userReputation.Reputation.Emote(500))} `500` репутации вы получите {_emotes.GetEmoteOrBlank(box.Emote())} {boxAmount} {_local.Localize(box.ToString(), boxAmount)}.\n" +
                $"{_emotes.GetEmoteOrBlank(userReputation.Amount >= 1000 ? "Checkmark" : "List")} За {_emotes.GetEmoteOrBlank(userReputation.Reputation.Emote(1000))} `1000` репутации вы получите {_emotes.GetEmoteOrBlank("Recipe")} {_local.Localize(food.Name)}.\n" +
                $"{_emotes.GetEmoteOrBlank(userReputation.Amount >= 2000 ? "Checkmark" : "List")} За {_emotes.GetEmoteOrBlank(userReputation.Reputation.Emote(2000))} `2000` репутации вы получите {_emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} {pearlAmount} {_local.Localize(Currency.Pearl.ToString(), pearlAmount)}.\n" +
                $"{_emotes.GetEmoteOrBlank(userReputation.Amount >= 5000 ? "Checkmark" : "List")} За {_emotes.GetEmoteOrBlank(userReputation.Reputation.Emote(5000))} `5000` репутации вы получите {card.Rarity.Localize(true)} [«{card.Name}»]({card.Url}).\n" +
                $"{_emotes.GetEmoteOrBlank(userReputation.Amount >= 10000 ? "Checkmark" : "List")} За {_emotes.GetEmoteOrBlank(userReputation.Reputation.Emote(10000))} `10000` репутации вы получите  титул {_emotes.GetEmoteOrBlank(title.Emote())} {title.Localize()}.";

            return rewardString;
        }
    }
}
