using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.TrainingService;
using Hinode.Izumi.Services.RpgServices.UserService;

namespace Hinode.Izumi.Services.Commands.UserCommands
{
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
    public class EatFoodCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IEmoteService _emoteService;
        private readonly IInventoryService _inventoryService;
        private readonly IFoodService _foodService;
        private readonly ILocalizationService _local;
        private readonly IUserService _userService;
        private readonly ITrainingService _trainingService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;

        public EatFoodCommand(IDiscordEmbedService discordEmbedService, IEmoteService emoteService,
            IInventoryService inventoryService, IFoodService foodService, ILocalizationService local,
            IUserService userService, ITrainingService trainingService, ICalculationService calc,
            IIngredientService ingredientService)
        {
            _discordEmbedService = discordEmbedService;
            _emoteService = emoteService;
            _inventoryService = inventoryService;
            _foodService = foodService;
            _local = local;
            _userService = userService;
            _trainingService = trainingService;
            _calc = calc;
            _ingredientService = ingredientService;
        }

        [Command("съесть"), Alias("eat")]
        public async Task EatFoodTask(long amount, [Remainder] string foodName)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // находим локализацию блюда
            var foodLocal = await _local.GetLocalizationByLocalizedWord(LocalizationCategory.Food, foodName);
            // получаем блюдо
            var food = await _foodService.GetFood(foodLocal.ItemId);
            // получаем блюдо у пользователя
            var userFood = await _inventoryService.GetUserFood((long) Context.User.Id, food.Id);

            // проверяем что у пользователя есть это блюдо в наличии
            if (userFood.Amount < amount)
            {
                await Task.FromException(new Exception(IzumiReplyMessage.EatFoodWrongAmount.Parse(
                    emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name))));
            }
            else
            {
                // считаем себестоимость блюда
                var costPrice = await _ingredientService.GetFoodCostPrice(food.Id);
                // считаем стоимость приготовления блюда
                var cookingPrice = await _calc.CraftingPrice(costPrice);
                // считаем количество энергии восстанавливаемой блюдом
                var foodEnergy = await _calc.FoodEnergyRecharge(costPrice, cookingPrice);

                // забираем у пользователя еду
                await _inventoryService.RemoveItemFromUser((long) Context.User.Id, InventoryCategory.Food, food.Id);
                // добавляем энергию пользователю
                await _userService.AddEnergyToUser((long) Context.User.Id, foodEnergy * amount);

                var embed = new EmbedBuilder()
                    // подверждаем что еда съедена и энергия добавлена
                    .WithDescription(IzumiReplyMessage.EatFoodSuccess.Parse(
                        emotes.GetEmoteOrBlank(food.Name), amount, _local.Localize(food.Name, amount),
                        emotes.GetEmoteOrBlank("Energy"), foodEnergy * amount,
                        _local.Localize("Energy", foodEnergy * amount)));

                await _discordEmbedService.SendEmbed(Context.User, embed);
                // проверяем нужно ли двинуть прогресс обучения пользователя
                if (food.Id == 4) await _trainingService.CheckStep((long) Context.User.Id, TrainingStep.EatFriedEgg);
                await Task.CompletedTask;
            }
        }
    }
}
