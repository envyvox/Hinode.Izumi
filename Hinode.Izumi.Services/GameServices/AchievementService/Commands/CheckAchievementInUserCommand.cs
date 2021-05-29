using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Services.GameServices.AchievementService.Queries;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CollectionService.Queries;
using Hinode.Izumi.Services.GameServices.CollectionService.Records;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.DrinkService.Queries;
using Hinode.Izumi.Services.GameServices.FishService.Queries;
using Hinode.Izumi.Services.GameServices.GatheringService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Commands
{
    public record CheckAchievementInUserCommand(long UserId, Achievement Type) : IRequest;

    public class CheckAchievementInUserHandler : IRequestHandler<CheckAchievementInUserCommand>
    {
        private readonly IMediator _mediator;

        public CheckAchievementInUserHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckAchievementInUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, type) = request;
            var achievement = await _mediator.Send(new GetAchievementByTypeQuery(type), cancellationToken);
            var hasAchievement = await _mediator.Send(
                new CheckUserHasAchievementQuery(userId, achievement.Id), cancellationToken);

            if (hasAchievement) return new Unit();

            UserStatisticRecord userStatistic;
            UserCollectionRecord[] userCollection;
            int collectionLength;

            switch (achievement.Type)
            {
                // достижения которые выполняются в один шаг
                case Achievement.FirstMessage:
                case Achievement.FirstTransit:
                case Achievement.FirstFish:
                case Achievement.FirstGatheringResource:
                case Achievement.FirstPlant:
                case Achievement.FirstCraftResource:
                case Achievement.FirstCook:
                case Achievement.FirstBet:
                case Achievement.FirstJackPot:
                case Achievement.FirstLotteryTicket:
                case Achievement.FirstMarketDeal:
                case Achievement.FirstContract:
                case Achievement.FirstGiftSent:
                case Achievement.FirstGiftReceived:
                case Achievement.CatchEpicFish:
                case Achievement.CatchMythicalFish:
                case Achievement.CatchLegendaryFish:
                case Achievement.CatchKoi:
                case Achievement.FirstCraftAlcohol:
                case Achievement.MeetSummer:

                    await _mediator.Send(
                        new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;

                // достижения которые связанны со статистикой
                case Achievement.Catch50Fish:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Fishing),
                        cancellationToken);

                    if (userStatistic?.Amount >= 50)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Catch300Fish:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Fishing),
                        cancellationToken);

                    if (userStatistic?.Amount >= 300)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Plant25Seed:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.SeedPlanted),
                        cancellationToken);

                    if (userStatistic?.Amount >= 25)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Plant150Seed:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.SeedPlanted),
                        cancellationToken);

                    if (userStatistic?.Amount >= 150)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Craft30Resource:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CraftingResource),
                        cancellationToken);

                    if (userStatistic?.Amount >= 30)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Craft250Resource:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CraftingResource),
                        cancellationToken);

                    if (userStatistic?.Amount >= 250)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Cook20Food:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Cooking),
                        cancellationToken);

                    if (userStatistic?.Amount >= 20)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Cook130Food:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Cooking),
                        cancellationToken);

                    if (userStatistic?.Amount >= 130)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Gather40Resources:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Gathering),
                        cancellationToken);

                    if (userStatistic?.Amount >= 40)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Gather250Resources:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.Gathering),
                        cancellationToken);

                    if (userStatistic?.Amount >= 250)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Craft10Alcohol:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CraftingAlcohol),
                        cancellationToken);

                    if (userStatistic?.Amount >= 10)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Craft80Alcohol:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CraftingAlcohol),
                        cancellationToken);

                    if (userStatistic?.Amount >= 80)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Collect50Crop:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CropHarvested),
                        cancellationToken);

                    if (userStatistic?.Amount >= 50)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Collect300Crop:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CropHarvested),
                        cancellationToken);

                    if (userStatistic?.Amount >= 300)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Casino33Bet:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CasinoBet),
                        cancellationToken);

                    if (userStatistic?.Amount >= 33)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Casino777Bet:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CasinoBet),
                        cancellationToken);

                    if (userStatistic?.Amount >= 777)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Casino22LotteryBuy:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CasinoLotteryBuy),
                        cancellationToken);

                    if (userStatistic?.Amount >= 22)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Casino99LotteryBuy:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CasinoLotteryBuy),
                        cancellationToken);

                    if (userStatistic?.Amount >= 99)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Casino20LotteryGift:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.CasinoLotteryGift),
                        cancellationToken);

                    if (userStatistic?.Amount >= 20)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Market100Sell:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.MarketSell),
                        cancellationToken);

                    if (userStatistic?.Amount >= 100)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Market666Sell:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.MarketSell),
                        cancellationToken);

                    if (userStatistic?.Amount >= 666)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Market50Buy:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.MarketBuy),
                        cancellationToken);

                    if (userStatistic?.Amount >= 50)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.Market333Buy:

                    userStatistic = await _mediator.Send(new GetUserStatisticQuery(userId, Statistic.MarketBuy),
                        cancellationToken);

                    if (userStatistic?.Amount >= 333)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;

                // достижения которые связанны с коллекцией
                case Achievement.CompleteCollectionGathering:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Gathering), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllGatheringsQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.CompleteCollectionCrafting:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Crafting), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllCraftingsQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.CompleteCollectionAlcohol:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Alcohol), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllAlcoholQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.CompleteCollectionCrop:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Crop), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllCropsQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.CompleteCollectionFish:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Fish), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllFishQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                case Achievement.CompleteCollectionFood:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Food), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllFishQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;

                case Achievement.CompleteCollectionDrink:

                    userCollection = await _mediator.Send(
                        new GetUserCollectionsQuery(userId, CollectionCategory.Drink), cancellationToken);
                    collectionLength = (await _mediator.Send(
                        new GetAllDrinksQuery(), cancellationToken)).Length;

                    if (userCollection.Length >= collectionLength)
                        await _mediator.Send(
                            new AddAchievementToUserCommand(userId, achievement.Id), cancellationToken);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Unit();
        }
    }
}
