using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Commands.UserCommands
{
    [CommandCategory(CommandCategory.Cooking)]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class EatFoodCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public EatFoodCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        [Command("съесть"), Alias("eat")]
        [Summary("Съесть блюдо с указанным названием")]
        [CommandUsage("!съесть 1 особый тыквенный пирог", "!съесть 5 особых тыквенных")]
        public async Task EatFoodCommandTask(
            [Summary("Количество")] long amount,
            [Summary("Название блюда")] [Remainder] string foodName)
        {
            // находим локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // пытаемся съесть
            await EatFoodTask(amount, foodLocal.ItemId);
        }

        [Command("съесть"), Alias("eat")]
        [Summary("Съесть блюдо с указанным номером")]
        [CommandUsage("!съесть 1 79", "!съесть 5 79")]
        public async Task EatFoodCommandTask(
            [Summary("Количество")] long amount,
            [Summary("Номер блюда")] long foodId) =>
            // пытаемся съесть
            await EatFoodTask(amount, foodId);

        private async Task EatFoodTask(long amount, long foodId)
        {
            // получаем иконки из базы
            var emotes = await _mediator.Send(new GetEmotesQuery());
            // получаем блюдо
            var food = await _mediator.Send(new GetFoodQuery(foodId));
            // получаем блюдо у пользователя
            var userFood = await _mediator.Send(new GetUserFoodQuery((long) Context.User.Id, food.Id));

            // проверяем что у пользователя есть это блюдо в наличии
            if (userFood.Amount < amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.EatFoodWrongAmount.Parse(
                    emotes.GetEmoteOrBlank(food.Name), _local.Localize(LocalizationCategory.Food, food.Id))));
            }
            else
            {
                // считаем себестоимость блюда
                var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Id));
                // считаем стоимость приготовления блюда
                var cookingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                // считаем количество энергии восстанавливаемой блюдом
                var foodEnergy = await _mediator.Send(new GetFoodEnergyRechargeQuery(costPrice, cookingPrice));

                // забираем у пользователя еду
                await _mediator.Send(new RemoveItemFromUserByInventoryCategoryCommand(
                    (long) Context.User.Id, InventoryCategory.Food, food.Id, amount));
                // добавляем энергию пользователю
                await _mediator.Send(new AddEnergyToUserCommand((long) Context.User.Id, foodEnergy * amount));

                var embed = new EmbedBuilder()
                    // подверждаем что еда съедена и энергия добавлена
                    .WithDescription(IzumiReplyMessage.EatFoodSuccess.Parse(
                        emotes.GetEmoteOrBlank(food.Name), amount,
                        _local.Localize(LocalizationCategory.Food, food.Id, amount), emotes.GetEmoteOrBlank("Energy"),
                        foodEnergy * amount, _local.Localize("Energy", foodEnergy * amount)));

                await _mediator.Send(new SendEmbedToUserCommand(Context.User, embed));

                // проверяем нужно ли двинуть прогресс обучения пользователя
                if (food.Id == 4) await _mediator.Send(new CheckUserTutorialStepCommand(
                        (long) Context.User.Id, TutorialStep.EatFriedEgg));

                await Task.CompletedTask;
            }
        }
    }
}
