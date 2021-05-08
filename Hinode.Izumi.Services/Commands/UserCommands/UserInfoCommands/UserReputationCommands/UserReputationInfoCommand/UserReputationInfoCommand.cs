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
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.CardService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.ReputationService;
using Hinode.Izumi.Services.RpgServices.ReputationService.Models;

namespace Hinode.Izumi.Services.Commands.UserCommands.UserInfoCommands.UserReputationCommands.UserReputationInfoCommand
{
    [InjectableService]
    public class UserReputationInfoCommand : IUserReputationInfoCommand
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IReputationService _reputationService;
        private readonly IFoodService _foodService;
        private readonly IPropertyService _propertyService;
        private readonly ILocalizationService _local;
        private readonly ICardService _cardService;

        private Dictionary<string, EmoteModel> _emotes;

        public UserReputationInfoCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IReputationService reputationService, IFoodService foodService, IPropertyService propertyService,
            ILocalizationService local, ICardService cardService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _reputationService = reputationService;
            _foodService = foodService;
            _propertyService = propertyService;
            _local = local;
            _cardService = cardService;
        }

        public async Task Execute(SocketCommandContext context, Reputation reputation)
        {
            _emotes = await _emoteService.GetEmotes();
            var userReputation = await _reputationService.GetUserReputation((long) context.User.Id, reputation);

            var embed = new EmbedBuilder()
                .WithDescription(
                    $"У вас {_emotes.GetEmoteOrBlank(reputation.Emote(userReputation.Amount))} {userReputation.Amount} репутации в **{reputation.Location().Localize(true)}**." +
                    $"\n{_emotes.GetEmoteOrBlank("Blank")}")
                .AddField("Награды за получение репутации", await DisplayRewards(userReputation));

            await _discordEmbedService.SendEmbed(context.User, embed);
            await Task.CompletedTask;
        }

        private async Task<string> DisplayRewards(UserReputationModel userReputation)
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
                    boxAmount = await _propertyService.GetPropertyValue(Property.ReputationCapitalBoxAmount);
                    foodId = await _propertyService.GetPropertyValue(Property.ReputationCapitalFoodId);
                    pearlAmount = await _propertyService.GetPropertyValue(Property.ReputationCapitalPearlAmount);
                    cardId = await _propertyService.GetPropertyValue(Property.ReputationCapitalCardId);
                    title = (Title) await _propertyService.GetPropertyValue(Property.ReputationCapitalTitleNumber);

                    break;
                case Reputation.Garden:

                    box = Box.Garden;
                    boxAmount = await _propertyService.GetPropertyValue(Property.ReputationGardenBoxAmount);
                    foodId = await _propertyService.GetPropertyValue(Property.ReputationGardenFoodId);
                    pearlAmount = await _propertyService.GetPropertyValue(Property.ReputationGardenPearlAmount);
                    cardId = await _propertyService.GetPropertyValue(Property.ReputationGardenCardId);
                    title = (Title) await _propertyService.GetPropertyValue(Property.ReputationGardenTitleNumber);

                    break;
                case Reputation.Seaport:

                    box = Box.Seaport;
                    boxAmount = await _propertyService.GetPropertyValue(Property.ReputationSeaportBoxAmount);
                    foodId = await _propertyService.GetPropertyValue(Property.ReputationSeaportFoodId);
                    pearlAmount = await _propertyService.GetPropertyValue(Property.ReputationSeaportPearlAmount);
                    cardId = await _propertyService.GetPropertyValue(Property.ReputationSeaportCardId);
                    title = (Title) await _propertyService.GetPropertyValue(Property.ReputationSeaportTitleNumber);

                    break;
                case Reputation.Castle:

                    box = Box.Castle;
                    boxAmount = await _propertyService.GetPropertyValue(Property.ReputationCastleBoxAmount);
                    foodId = await _propertyService.GetPropertyValue(Property.ReputationCastleFoodId);
                    pearlAmount = await _propertyService.GetPropertyValue(Property.ReputationCastlePearlAmount);
                    cardId = await _propertyService.GetPropertyValue(Property.ReputationCastleCardId);
                    title = (Title) await _propertyService.GetPropertyValue(Property.ReputationCastleTitleNumber);

                    break;
                case Reputation.Village:

                    box = Box.Village;
                    boxAmount = await _propertyService.GetPropertyValue(Property.ReputationVillageBoxAmount);
                    foodId = await _propertyService.GetPropertyValue(Property.ReputationVillageFoodId);
                    pearlAmount = await _propertyService.GetPropertyValue(Property.ReputationVillagePearlAmount);
                    cardId = await _propertyService.GetPropertyValue(Property.ReputationVillageCardId);
                    title = (Title) await _propertyService.GetPropertyValue(Property.ReputationVillageTitleNumber);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var food = await _foodService.GetFood(foodId);
            var card = await _cardService.GetCard(cardId);

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
