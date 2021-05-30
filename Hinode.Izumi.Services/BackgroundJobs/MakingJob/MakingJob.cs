using System;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CollectionService.Commands;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.LocationService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Commands;
using Hinode.Izumi.Services.GameServices.MasteryService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.HangfireJobService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Services.BackgroundJobs.MakingJob
{
    [InjectableService]
    public class MakingJob : IMakingJob
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public MakingJob(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }


        public async Task CompleteCrafting(long userId, long craftingId, long amount, Location location)
        {
            // получаем изготавливаемый предмет
            var crafting = await _mediator.Send(new GetCraftingQuery(craftingId));
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем мастерство изготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _mediator.Send(new GetUserMasteryQuery(userId, Mastery.Crafting)))
                .Amount);
            // определяем количество итоговых изготовленных предметов после проков мастерства
            var amountAfterProcs = await _mediator.Send(new GetCraftingAmountAfterMasteryProcsQuery(
                crafting.Id, userMastery, amount));

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, location));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю изготовленные предметы
            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                userId, InventoryCategory.Crafting, crafting.Id, amountAfterProcs));
            // добавляем пользователь запись в коллекции
            await _mediator.Send(new AddCollectionToUserCommand(
                userId, CollectionCategory.Crafting, crafting.Id));
            // добавляем пользователю мастерство изготовления
            await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Crafting,
                // определяем количество полученного мастерства изготовления
                await _mediator.Send(new GetMasteryXpQuery(
                    MasteryXpProperty.CraftingResource, userMastery, amount))));
            // добавляем пользователю статистику созданных предметов
            await _mediator.Send(new AddStatisticToUserCommand(
                userId, Statistic.CraftingResource, amountAfterProcs));
            // проверяем выполнил ли пользователь достижения
            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                Achievement.FirstCraftResource,
                Achievement.Craft30Resource,
                Achievement.Craft250Resource,
                Achievement.CompleteCollectionCrafting
            }));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingCrafting.Localize())
                // баннер изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // оповещаем о окончании процесса изготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingResourceCompleteDesc.Parse(
                        emotes.GetEmoteOrBlank(crafting.Name), _local.Localize(crafting.Name, 2)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученные предметы
                .AddField(IzumiReplyMessage.CraftingResourceReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(crafting.Name)} {amountAfterProcs} {_local.Localize(crafting.Name, amountAfterProcs)}");

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Making));
            await Task.CompletedTask;
        }

        public async Task CompleteAlcohol(long userId, long alcoholId, long amount, Location location)
        {
            // получаем изготавливаемый алкоголь
            var alcohol = await _mediator.Send(new GetAlcoholQuery(alcoholId));
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем мастерство изготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _mediator.Send(new GetUserMasteryQuery(userId, Mastery.Crafting)))
                .Amount);
            // определяем количество изготовленного алкоголя после проков мастерства
            var amountAfterProcs = await _mediator.Send(new GetAlcoholAmountAfterMasteryProcsQuery(
                alcohol.Id, userMastery, amount));

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, location));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю изготовленный алкоголь
            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                userId, InventoryCategory.Alcohol,
                alcohol.Id, amountAfterProcs));
            // добавляем пользователю запись в коллекцию
            await _mediator.Send(new AddCollectionToUserCommand(
                userId, CollectionCategory.Alcohol, alcohol.Id));
            // добавляем пользователю мастерство изготовление
            await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Crafting,
                // определяем количество полученного мастерства изготовления
                await _mediator.Send(new GetMasteryXpQuery(
                    MasteryXpProperty.CraftingAlcohol, userMastery, amount))));
            // добавляем пользователю статистику изготовленного алкоголя
            await _mediator.Send(new AddStatisticToUserCommand(
                userId, Statistic.CraftingAlcohol, amountAfterProcs));
            // проверяем выполнил ли пользователь достижения
            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                Achievement.FirstCraftAlcohol,
                Achievement.Craft10Alcohol,
                Achievement.Craft80Alcohol,
                Achievement.CompleteCollectionAlcohol
            }));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingAlcohol.Localize())
                // баннер изготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Crafting)))
                // оповещаем о окончании изготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingAlcoholCompleteDesc.Parse(
                        emotes.GetEmoteOrBlank(alcohol.Name), _local.Localize(alcohol.Name)) +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученный алкоголь
                .AddField(IzumiReplyMessage.CraftingAlcoholReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(alcohol.Name)} {amountAfterProcs} {_local.Localize(alcohol.Name, amountAfterProcs)}");

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Making));
            await Task.CompletedTask;
        }

        public async Task CompleteFood(long userId, long foodId, long amount, Location location)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем приготавливаемую еду
            var food = await _mediator.Send(new GetFoodQuery(foodId));
            // получаем мастерство приготовления пользователя и округляем его
            var userMastery = (long) Math.Floor(
                (await _mediator.Send(new GetUserMasteryQuery(userId, Mastery.Cooking)))
                .Amount);

            // обновляем текущую локацию пользователя
            await _mediator.Send(new UpdateUserLocationCommand(userId, location));
            // удаляем информацию о перемещении
            await _mediator.Send(new DeleteUserMovementCommand(userId));
            // добавляем пользователю приготовленную еду
            await _mediator.Send(
                new AddItemToUserByInventoryCategoryCommand(userId, InventoryCategory.Food, foodId, amount));
            // добавляем пользователю запись в коллекцию
            await _mediator.Send(new AddCollectionToUserCommand(userId, CollectionCategory.Food, foodId));
            // добавляем пользователю мастерство приготовления
            await _mediator.Send(new AddMasteryToUserCommand(userId, Mastery.Cooking,
                // определяем количество полученного мастерства приготовления
                await _mediator.Send(new GetMasteryXpQuery(MasteryXpProperty.Cooking, userMastery, amount))));
            // добавляем пользователю статистику приготовленного блюда
            await _mediator.Send(new AddStatisticToUserCommand(userId, Statistic.Cooking, amount));
            // добавляем пользователю статистику приготовленного блюда по категории мастерства
            await _mediator.Send(new AddStatisticToUserCommand(userId, food.Mastery switch
            {
                0 => Statistic.CookingBeginner,
                50 => Statistic.CookingApprentice,
                100 => Statistic.CookingExperienced,
                150 => Statistic.CookingProfessional,
                200 => Statistic.CookingExpert,
                250 => Statistic.CookingMaster,
                _ => throw new ArgumentOutOfRangeException()
            }, amount));
            // проверяем выполнил ли пользователь достижения
            await _mediator.Send(new CheckAchievementsInUserCommand(userId, new[]
            {
                Achievement.FirstCook,
                Achievement.CompleteCollectionFood
            }));

            var embed = new EmbedBuilder()
                .WithAuthor(Location.MakingFood.Localize())
                // баннер приготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Cooking)))
                // оповещаем о окончании приготовления
                .WithDescription(
                    IzumiReplyMessage.CraftingFoodCompleteDesc.Parse() +
                    $"\n{emotes.GetEmoteOrBlank("Blank")}")
                // полученная еда
                .AddField(IzumiReplyMessage.CraftingFoodReceivedFieldName.Parse(),
                    $"{emotes.GetEmoteOrBlank(food.Name)} {amount} {_local.Localize(food.Name)}");

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(userId)), embed));
            // проверяем нужно ли двинуть прогресс обучения пользователя
            if (food.Id == 4)
            {
                await _mediator.Send(new CheckUserTutorialStepCommand(userId, TutorialStep.CookFriedEgg));
            }
            await _mediator.Send(new DeleteUserHangfireJobCommand(userId, HangfireAction.Making));
            await Task.CompletedTask;
        }
    }
}
