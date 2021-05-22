using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Data.Enums.RarityEnums;
using Hinode.Izumi.Data.Enums.ReputationEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CardService.Commands;
using Hinode.Izumi.Services.GameServices.CardService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.ReputationService.Commands;
using Hinode.Izumi.Services.GameServices.ReputationService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReputationService.Handlers
{
    public class CheckReputationRewardsHandler : IRequestHandler<CheckReputationRewardsCommand>
    {
        private readonly IMediator _mediator;
        private readonly IConnectionManager _con;
        private readonly ILocalizationService _local;

        public CheckReputationRewardsHandler(IMediator mediator, IConnectionManager con, ILocalizationService local)
        {
            _mediator = mediator;
            _con = con;
            _local = local;
        }

        public async Task<Unit> Handle(CheckReputationRewardsCommand request, CancellationToken cancellationToken)
        {
            var (userId, reputation) = request;
            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var userReputation = await _mediator.Send(
                new GetUserReputationQuery(userId, reputation), cancellationToken);

            switch (userReputation.Amount)
            {
                case >= 10000:

                    var check10000RepReward = await _mediator.Send(
                        new CheckUserReputationRewardQuery(userId, reputation, 10000), cancellationToken);

                    if (check10000RepReward) return new Unit();

                    var title = (Title) await _mediator.Send(new GetPropertyValueQuery(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalTitleNumber,
                        Reputation.Garden => Property.ReputationGardenTitleNumber,
                        Reputation.Seaport => Property.ReputationSeaportTitleNumber,
                        Reputation.Castle => Property.ReputationCastleTitleNumber,
                        Reputation.Village => Property.ReputationVillageTitleNumber,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    }), cancellationToken);

                    await _mediator.Send(new AddTitleToUserCommand(userId, title), cancellationToken);
                    await AddUserReputationReward(userId, reputation, 10000);
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(10000)), 10000, reputation.Location().Localize(true),
                        "титул", emotes.GetEmoteOrBlank(title.Emote()), title.Localize()));

                    break;
                case >= 5000:

                    var check5000RepReward = await _mediator.Send(
                        new CheckUserReputationRewardQuery(userId, reputation, 5000), cancellationToken);

                    if (check5000RepReward) return new Unit();

                    var cardId = await _mediator.Send(new GetPropertyValueQuery(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalCardId,
                        Reputation.Garden => Property.ReputationGardenCardId,
                        Reputation.Seaport => Property.ReputationSeaportCardId,
                        Reputation.Castle => Property.ReputationCastleCardId,
                        Reputation.Village => Property.ReputationVillageCardId,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    }), cancellationToken);
                    var card = await _mediator.Send(new GetCardQuery(cardId), cancellationToken);

                    await _mediator.Send(new AddCardToUserCommand(userId, card.Id), cancellationToken);
                    await AddUserReputationReward(userId, reputation, 5000);
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(5000)), 5000, reputation.Location().Localize(true),
                        "", card.Rarity.Localize(true), $"[{card.Name}]({card.Url})"));

                    break;
                case >= 2000:

                    var check2000RepReward = await _mediator.Send(
                        new CheckUserReputationRewardQuery(userId, reputation, 2000), cancellationToken);

                    if (check2000RepReward) return new Unit();

                    var pearlAmount = await _mediator.Send(new GetPropertyValueQuery(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalPearlAmount,
                        Reputation.Garden => Property.ReputationGardenPearlAmount,
                        Reputation.Seaport => Property.ReputationSeaportPearlAmount,
                        Reputation.Castle => Property.ReputationCastlePearlAmount,
                        Reputation.Village => Property.ReputationVillagePearlAmount,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    }), cancellationToken);

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                            userId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), pearlAmount),
                        cancellationToken);
                    await AddUserReputationReward(userId, reputation, 2000);
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(2000)), 2000, reputation.Location().Localize(true),
                        emotes.GetEmoteOrBlank(Currency.Pearl.ToString()), pearlAmount,
                        _local.Localize(Currency.Pearl.ToString(), pearlAmount)));

                    break;
                case >= 1000:

                    var check1000RepReward = await _mediator.Send(
                        new CheckUserReputationRewardQuery(userId, reputation, 1000), cancellationToken);

                    if (check1000RepReward) return new Unit();

                    var foodId = await _mediator.Send(new GetPropertyValueQuery(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalFoodId,
                        Reputation.Garden => Property.ReputationGardenFoodId,
                        Reputation.Seaport => Property.ReputationSeaportFoodId,
                        Reputation.Castle => Property.ReputationCastleFoodId,
                        Reputation.Village => Property.ReputationVillageFoodId,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    }), cancellationToken);

                    await _mediator.Send(new AddRecipeToUserCommand(userId, foodId), cancellationToken);
                    await AddUserReputationReward(userId, reputation, 1000);
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(1000)), 1000, reputation.Location().Localize(true),
                        "", emotes.GetEmoteOrBlank("Recipe"), _local.Localize(LocalizationCategory.Food, foodId)));

                    break;
                case >= 500:

                    var check500RepReward = await _mediator.Send(
                        new CheckUserReputationRewardQuery(userId, reputation, 500), cancellationToken);

                    if (check500RepReward) return new Unit();

                    var box = reputation switch
                    {
                        Reputation.Capital => Box.Capital,
                        Reputation.Garden => Box.Garden,
                        Reputation.Seaport => Box.Seaport,
                        Reputation.Castle => Box.Castle,
                        Reputation.Village => Box.Village,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    };
                    var boxAmount = await _mediator.Send(new GetPropertyValueQuery(reputation switch
                    {
                        Reputation.Capital => Property.ReputationCapitalBoxAmount,
                        Reputation.Garden => Property.ReputationGardenBoxAmount,
                        Reputation.Seaport => Property.ReputationSeaportBoxAmount,
                        Reputation.Castle => Property.ReputationCastleBoxAmount,
                        Reputation.Village => Property.ReputationVillageBoxAmount,
                        _ => throw new ArgumentOutOfRangeException(nameof(reputation), reputation, null)
                    }), cancellationToken);

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                            userId, InventoryCategory.Box, box.GetHashCode(), boxAmount),
                        cancellationToken);
                    await AddUserReputationReward(userId, reputation, 500);
                    await NotifyUserReputationReward(userId, IzumiReplyMessage.ReputationRewardDesc.Parse(
                        emotes.GetEmoteOrBlank(reputation.Emote(500)), 500, reputation.Location().Localize(true),
                        emotes.GetEmoteOrBlank(box.Emote()), boxAmount, _local.Localize(box.ToString(), boxAmount)));

                    break;
            }

            return new Unit();
        }

        private async Task AddUserReputationReward(long userId, Reputation reputation, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_reputation_rewards(user_id, reputation, amount)
                    values (@userId, @reputation, @amount)",
                    new {userId, reputation, amount});

        private async Task NotifyUserReputationReward(long userId, string message)
        {
            var embed = new EmbedBuilder()
                .WithAuthor(IzumiReplyMessage.ReputationRewardAuthor.Parse())
                .WithDescription(message);

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
        }
    }
}
