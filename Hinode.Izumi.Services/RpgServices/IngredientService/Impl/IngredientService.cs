using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.AlcoholService;
using Hinode.Izumi.Services.RpgServices.CraftingService;
using Hinode.Izumi.Services.RpgServices.CropService;
using Hinode.Izumi.Services.RpgServices.DrinkService;
using Hinode.Izumi.Services.RpgServices.FoodService;
using Hinode.Izumi.Services.RpgServices.GatheringService;
using Hinode.Izumi.Services.RpgServices.IngredientService.Models;
using Hinode.Izumi.Services.RpgServices.InventoryService;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.ProductService;

namespace Hinode.Izumi.Services.RpgServices.IngredientService.Impl
{
    [InjectableService]
    public class IngredientService : IIngredientService
    {
        private readonly IConnectionManager _con;
        private readonly IInventoryService _inventoryService;
        private readonly IGatheringService _gatheringService;
        private readonly ICraftingService _craftingService;
        private readonly IProductService _productService;
        private readonly IAlcoholService _alcoholService;
        private readonly IDrinkService _drinkService;
        private readonly ICropService _cropService;
        private readonly IFoodService _foodService;
        private readonly ILocalizationService _local;
        private readonly IEmoteService _emoteService;

        public IngredientService(IConnectionManager con, IInventoryService inventoryService,
            IGatheringService gatheringService, ICraftingService craftingService, IProductService productService,
            IAlcoholService alcoholService, IDrinkService drinkService, ICropService cropService,
            IFoodService foodService, ILocalizationService local, IEmoteService emoteService)
        {
            _con = con;
            _inventoryService = inventoryService;
            _gatheringService = gatheringService;
            _craftingService = craftingService;
            _productService = productService;
            _alcoholService = alcoholService;
            _drinkService = drinkService;
            _cropService = cropService;
            _foodService = foodService;
            _local = local;
            _emoteService = emoteService;
        }

        public async Task<CraftingIngredientModel[]> GetCraftingIngredients(long craftingId) =>
            (await _con.GetConnection()
                .QueryAsync<CraftingIngredientModel>(@"
                    select * from crafting_ingredients
                    where crafting_id = @craftingId
                    order by ingredient_id",
                    new {craftingId}))
            .ToArray();

        public async Task<AlcoholIngredientModel[]> GetAlcoholIngredients(long alcoholId) =>
            (await _con.GetConnection()
                .QueryAsync<AlcoholIngredientModel>(@"
                    select * from alcohol_ingredients
                    where alcohol_id = @alcoholId
                    order by ingredient_id",
                    new {alcoholId}))
            .ToArray();

        public async Task<DrinkIngredientModel[]> GetDrinkIngredients(long drinkId) =>
            (await _con.GetConnection()
                .QueryAsync<DrinkIngredientModel>(@"
                    select * from drink_ingredients
                    where drink_id = @drinkId
                    order by ingredient_id",
                    new {drinkId}))
            .ToArray();

        public async Task<FoodIngredientModel[]> GetFoodIngredients(long foodId) =>
            (await _con.GetConnection()
                .QueryAsync<FoodIngredientModel>(@"
                    select * from food_ingredients
                    where food_id = @foodId
                    order by ingredient_id",
                    new {foodId}))
            .ToArray();

        public async Task<ProjectIngredientModel[]> GetProjectIngredients(long projectId) =>
            (await _con.GetConnection()
                .QueryAsync<ProjectIngredientModel>(@"
                    select * from project_ingredients
                    where project_id = @projectId
                    order by ingredient_id",
                    new {projectId}))
            .ToArray();

        public async Task<List<Season>> GetFoodSeasons(long foodId)
        {
            var ingredients = await GetFoodIngredients(foodId);
            var seasons = new List<Season>();

            foreach (var ingredient in ingredients)
            {
                var ingredientSeasons = await GetIngredientSeasons(ingredient.Category, ingredient.IngredientId);
                foreach (var season in ingredientSeasons)
                {
                    if (!seasons.Contains(season)) seasons.Add(season);
                }
            }

            seasons.Sort();
            return seasons;
        }

        public async Task<long> GetCraftingCostPrice(long craftingId)
        {
            // получаем ингредиенты изготавливаемого предмета
            var ingredients = await GetCraftingIngredients(craftingId);
            long costPrice = 0;

            // для каждого ингредиента получаем его стоимость
            foreach (var ingredient in ingredients)
                costPrice += await GetIngredientCostPrice(ingredient.Category, ingredient.IngredientId) *
                             ingredient.Amount;

            return costPrice;
        }

        public async Task<long> GetAlcoholCostPrice(long alcoholId)
        {
            // получаем ингредиенты алкоголя
            var ingredients = await GetAlcoholIngredients(alcoholId);
            long costPrice = 0;

            // для каждого ингредиента получаем его стоимость
            foreach (var ingredient in ingredients)
                costPrice += await GetIngredientCostPrice(ingredient.Category, ingredient.IngredientId) *
                             ingredient.Amount;
            ;

            return costPrice;
        }

        public async Task<long> GetDrinkCostPrice(long drinkId)
        {
            // получаем ингредиенты напитка
            var ingredients = await GetDrinkIngredients(drinkId);
            long costPrice = 0;

            // для каждого ингредиента получаем его стоимость
            foreach (var ingredient in ingredients)
                costPrice += await GetIngredientCostPrice(ingredient.Category, ingredient.IngredientId) *
                             ingredient.Amount;
            ;

            return costPrice;
        }

        public async Task<long> GetFoodCostPrice(long foodId)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetFoodIngredients(foodId);
            long costPrice = 0;

            // для каждого ингредиента получаем его стоимость
            foreach (var ingredient in ingredients)
                costPrice += await GetIngredientCostPrice(ingredient.Category, ingredient.IngredientId) *
                             ingredient.Amount;
            ;

            return costPrice;
        }

        public async Task<long> GetProjectCostPrice(long projectId)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetProjectIngredients(projectId);
            long costPrice = 0;

            // для каждого ингредиента получаем его стоимость
            foreach (var ingredient in ingredients)
                costPrice += await GetIngredientCostPrice(ingredient.Category, ingredient.IngredientId) *
                             ingredient.Amount;
            ;

            return costPrice;
        }

        public async Task<string> DisplayCraftingIngredients(long craftingId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем ингредиенты изготавливаемого предмета
            var ingredients = await GetCraftingIngredients(craftingId);

            // для каждого ингредиента необходимо добавить локализированную информацию
            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(ingredient.Category, ingredient.IngredientId);

                // добавляем локализированную информацию об ингредиенте
                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount * amount} {_local.Localize(ingredientName, ingredient.Amount * amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }

        public async Task<string> DisplayAlcoholIngredients(long alcoholId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем ингредиенты алкоголя
            var ingredients = await GetAlcoholIngredients(alcoholId);

            // для каждого ингредиента необходимо добавить локализированную информацию
            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(ingredient.Category, ingredient.IngredientId);

                // добавляем локализированную информацию об ингредиенте
                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount * amount} {_local.Localize(ingredientName, ingredient.Amount * amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }

        public async Task<string> DisplayDrinkIngredients(long drinkId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем ингредиенты алкоголя
            var ingredients = await GetDrinkIngredients(drinkId);

            // для каждого ингредиента необходимо добавить локализированную информацию
            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(ingredient.Category, ingredient.IngredientId);

                // добавляем локализированную информацию об ингредиенте
                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount * amount} {_local.Localize(ingredientName, ingredient.Amount * amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }

        public async Task<string> DisplayFoodIngredients(long foodId, long amount = 1)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем ингредиенты блюда
            var ingredients = await GetFoodIngredients(foodId);

            // для каждого ингредиента необходимо добавить локализированную информацию
            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(ingredient.Category, ingredient.IngredientId);

                // добавляем локализированную информацию об ингредиенте
                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount * amount} {_local.Localize(ingredientName, ingredient.Amount * amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }

        public async Task<string> DisplayProjectIngredients(long projectId)
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем ингредиенты блюда
            var ingredients = await GetProjectIngredients(projectId);

            // для каждого ингредиента необходимо добавить локализированную информацию
            var ingredientsString = string.Empty;
            foreach (var ingredient in ingredients)
            {
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(ingredient.Category, ingredient.IngredientId);

                // добавляем локализированную информацию об ингредиенте
                ingredientsString +=
                    $"{emotes.GetEmoteOrBlank(ingredientName)} {ingredient.Amount} {_local.Localize(ingredientName, ingredient.Amount)}, ";
            }

            return ingredientsString.Remove(ingredientsString.Length - 2);
        }

        public async Task CheckCraftingIngredients(long userId, long craftingId, long amount = 1)
        {
            // получаем ингредиенты изготавливаемого предмета
            var ingredients = await GetCraftingIngredients(craftingId);
            // проверяем есть ли каждый из ингредиентов в инвентаре пользователя
            foreach (var ingredient in ingredients)
                await CheckIngredientAmount(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount);
        }

        public async Task CheckAlcoholIngredients(long userId, long alcoholId, long amount = 1)
        {
            // получаем ингредиенты алкоголя
            var ingredients = await GetAlcoholIngredients(alcoholId);
            // проверяем есть ли каждый из ингредиентов в инвентаре пользователя
            foreach (var ingredient in ingredients)
                await CheckIngredientAmount(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount);
        }

        public async Task CheckDrinkIngredients(long userId, long drinkId, long amount = 1)
        {
            // получаем ингредиенты напитка
            var ingredients = await GetDrinkIngredients(drinkId);
            // проверяем есть ли каждый из ингредиентов в инвентаре пользователя
            foreach (var ingredient in ingredients)
                await CheckIngredientAmount(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount);
        }

        public async Task CheckFoodIngredients(long userId, long foodId, long amount = 1)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetFoodIngredients(foodId);
            // проверяем есть ли каждый из ингредиентов в инвентаре пользователя
            foreach (var ingredient in ingredients)
                await CheckIngredientAmount(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount, amount);
        }

        public async Task CheckProjectIngredients(long userId, long projectId)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetProjectIngredients(projectId);
            // проверяем есть ли каждый из ингредиентов в инвентаре пользователя
            foreach (var ingredient in ingredients)
                await CheckIngredientAmount(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount);
        }

        public async Task RemoveCraftingIngredients(long userId, long craftingId, long amount = 1)
        {
            // получаем ингредиенты изготавливаемого предмета
            var ingredients = await GetCraftingIngredients(craftingId);
            // отнимаем из инвентаря пользователя каждый из них
            foreach (var ingredient in ingredients)
            {
                // отнимаем из инвентаря пользователя ингредиент
                await _inventoryService.RemoveItemFromUser(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount * amount);
            }
        }

        public async Task RemoveAlcoholIngredients(long userId, long alcoholId, long amount = 1)
        {
            // получаем ингредиенты алкоголя
            var ingredients = await GetAlcoholIngredients(alcoholId);
            // отнимаем из инвентаря пользователя каждый из них
            foreach (var ingredient in ingredients)
            {
                // отнимаем из инвентаря пользователя ингредиент
                await _inventoryService.RemoveItemFromUser(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount * amount);
            }
        }

        public async Task RemoveDrinkIngredients(long userId, long drinkId, long amount = 1)
        {
            // получаем ингредиенты напитка
            var ingredients = await GetDrinkIngredients(drinkId);
            // отнимаем из инвентаря пользователя каждый из них
            foreach (var ingredient in ingredients)
            {
                // отнимаем из инвентаря пользователя ингредиент
                await _inventoryService.RemoveItemFromUser(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount * amount);
            }
        }

        public async Task RemoveFoodIngredients(long userId, long foodId, long amount = 1)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetFoodIngredients(foodId);
            // отнимаем из инвентаря пользователя каждый из них
            foreach (var ingredient in ingredients)
            {
                // отнимаем из инвентаря пользователя ингредиент
                await _inventoryService.RemoveItemFromUser(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount * amount);
            }
        }

        public async Task RemoveProjectIngredients(long userId, long projectId)
        {
            // получаем ингредиенты блюда
            var ingredients = await GetProjectIngredients(projectId);
            // отнимаем из инвентаря пользователя каждый из них
            foreach (var ingredient in ingredients)
            {
                // отнимаем из инвентаря пользователя ингредиент
                await _inventoryService.RemoveItemFromUser(
                    userId, ingredient.Category, ingredient.IngredientId, ingredient.Amount);
            }
        }

        /// <summary>
        /// Проверяет есть ли в инвентаре пользователя необходимый ингредиент.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="category">Категория ингредиента.</param>
        /// <param name="ingredientId">Id ингредиента.</param>
        /// <param name="ingredientAmount">Количество ингредиентов.</param>
        /// <param name="craftingAmount">Количество изготовлений (по-умолчанию 1).</param>
        /// <exception cref="IzumiReplyMessage.NoRequiredIngredientAmount"></exception>
        private async Task CheckIngredientAmount(long userId, IngredientCategory category, long ingredientId,
            long ingredientAmount, long craftingAmount = 1)
        {
            long userAmount;
            // получаем количество ингредиента в инвентаре пользователя в зависимости от категории ингредиента
            switch (category)
            {
                case IngredientCategory.Gathering:
                    var userGathering = await _inventoryService.GetUserGathering(userId, ingredientId);
                    userAmount = userGathering.Amount;
                    break;
                case IngredientCategory.Product:
                    var userProduct = await _inventoryService.GetUserProduct(userId, ingredientId);
                    userAmount = userProduct.Amount;
                    break;
                case IngredientCategory.Crafting:
                    var userCrafting = await _inventoryService.GetUserCrafting(userId, ingredientId);
                    userAmount = userCrafting.Amount;
                    break;
                case IngredientCategory.Alcohol:
                    var userAlcohol = await _inventoryService.GetUserAlcohol(userId, ingredientId);
                    userAmount = userAlcohol.Amount;
                    break;
                case IngredientCategory.Drink:
                    var userDrink = await _inventoryService.GetUserDrink(userId, ingredientId);
                    userAmount = userDrink.Amount;
                    break;
                case IngredientCategory.Crop:
                    var userCrop = await _inventoryService.GetUserCrop(userId, ingredientId);
                    userAmount = userCrop.Amount;
                    break;
                case IngredientCategory.Food:
                    var userFood = await _inventoryService.GetUserFood(userId, ingredientId);
                    userAmount = userFood.Amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            // если у пользователя недостаточное количество ингредиента
            if (userAmount < ingredientAmount * craftingAmount)
            {
                // получаем иконки из базы
                var emotes = await _emoteService.GetEmotes();
                // получаем название ингредиента
                var ingredientName = await GetIngredientName(category, ingredientId);
                // возвращаем ошибку
                await Task.FromException(new Exception(IzumiReplyMessage.NoRequiredIngredientAmount.Parse(
                    emotes.GetEmoteOrBlank(ingredientName), _local.Localize(ingredientName, 5))));
            }
        }

        /// <summary>
        /// Возвращает название ингредиента.
        /// </summary>
        /// <param name="category">Категория ингредиента.</param>
        /// <param name="ingredientId">Id ингредиента.</param>
        /// <returns>Название ингредиента.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private async Task<string> GetIngredientName(IngredientCategory category, long ingredientId)
        {
            string name;
            // получаем ингредиент из базы в зависимости от категории ингредиента
            switch (category)
            {
                case IngredientCategory.Gathering:
                    var gathering = await _gatheringService.GetGathering(ingredientId);
                    name = gathering.Name;
                    break;
                case IngredientCategory.Product:
                    var product = await _productService.GetProduct(ingredientId);
                    name = product.Name;
                    break;
                case IngredientCategory.Crafting:
                    var crafting = await _craftingService.GetCrafting(ingredientId);
                    name = crafting.Name;
                    break;
                case IngredientCategory.Alcohol:
                    var alcohol = await _alcoholService.GetAlcohol(ingredientId);
                    name = alcohol.Name;
                    break;
                case IngredientCategory.Drink:
                    var drink = await _drinkService.GetDrink(ingredientId);
                    name = drink.Name;
                    break;
                case IngredientCategory.Crop:
                    var crop = await _cropService.GetCrop(ingredientId);
                    name = crop.Name;
                    break;
                case IngredientCategory.Food:
                    var food = await _foodService.GetFood(ingredientId);
                    name = food.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            return name;
        }

        /// <summary>
        /// Возвращает себестоимость ингредиента.
        /// </summary>
        /// <param name="category">Категория ингредиента.</param>
        /// <param name="ingredientId">Id ингредиента.</param>
        /// <returns>Себестоимость ингредиента.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private async Task<long> GetIngredientCostPrice(IngredientCategory category, long ingredientId)
        {
            long costPrice = 0;
            switch (category)
            {
                case IngredientCategory.Gathering:
                    var gathering = await _gatheringService.GetGathering(ingredientId);
                    costPrice += gathering.Price;
                    break;
                case IngredientCategory.Product:
                    var product = await _productService.GetProduct(ingredientId);
                    costPrice += product.Price;
                    break;
                case IngredientCategory.Crafting:
                    costPrice += await GetCraftingCostPrice(ingredientId);
                    break;
                case IngredientCategory.Alcohol:
                    costPrice += await GetAlcoholCostPrice(ingredientId);
                    break;
                case IngredientCategory.Drink:
                    costPrice += await GetDrinkCostPrice(ingredientId);
                    break;
                case IngredientCategory.Crop:
                    var crop = await _cropService.GetCrop(ingredientId);
                    costPrice += crop.Price;
                    break;
                case IngredientCategory.Food:
                    costPrice += await GetFoodCostPrice(ingredientId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return costPrice;
        }

        private async Task<List<Season>> GetIngredientSeasons(IngredientCategory category, long ingredientId)
        {
            var seasons = new List<Season>();
            switch (category)
            {
                // собирательские предметы, продукты и морепродукты игнорируются, т.к. не имеют привязки к сезону
                case IngredientCategory.Gathering:
                case IngredientCategory.Product:
                case IngredientCategory.Seafood:
                    break;
                case IngredientCategory.Crafting:

                    var craftingIngredients = await GetCraftingIngredients(ingredientId);
                    var craftingSeasons = new List<Season>();

                    foreach (var craftingIngredient in craftingIngredients)
                    {
                        var craftingIngredientSeasons = await GetIngredientSeasons(
                            craftingIngredient.Category, craftingIngredient.IngredientId);

                        foreach (var craftingIngredientSeason in craftingIngredientSeasons)
                        {
                            if (!craftingSeasons.Contains(craftingIngredientSeason))
                                craftingSeasons.Add(craftingIngredientSeason);
                        }
                    }

                    foreach (var craftingSeason in craftingSeasons)
                    {
                        if (!seasons.Contains(craftingSeason))
                            seasons.Add(craftingSeason);
                    }

                    break;
                case IngredientCategory.Alcohol:

                    var alcoholIngredients = await GetAlcoholIngredients(ingredientId);
                    var alcoholSeasons = new List<Season>();

                    foreach (var alcoholIngredient in alcoholIngredients)
                    {
                        var alcoholIngredientSeasons = await GetIngredientSeasons(
                            alcoholIngredient.Category, alcoholIngredient.IngredientId);

                        foreach (var alcoholIngredientSeason in alcoholIngredientSeasons)
                        {
                            if (!alcoholSeasons.Contains(alcoholIngredientSeason))
                                alcoholSeasons.Add(alcoholIngredientSeason);
                        }
                    }

                    foreach (var alcoholSeason in alcoholSeasons)
                    {
                        if (!seasons.Contains(alcoholSeason))
                            seasons.Add(alcoholSeason);
                    }

                    break;
                case IngredientCategory.Drink:

                    var drinkIngredients = await GetDrinkIngredients(ingredientId);
                    var drinkSeasons = new List<Season>();

                    foreach (var drinkIngredient in drinkIngredients)
                    {
                        var drinkIngredientSeasons = await GetIngredientSeasons(
                            drinkIngredient.Category, drinkIngredient.IngredientId);

                        foreach (var drinkIngredientSeason in drinkIngredientSeasons)
                        {
                            if (!drinkSeasons.Contains(drinkIngredientSeason))
                                drinkSeasons.Add(drinkIngredientSeason);
                        }
                    }

                    foreach (var drinkSeason in drinkSeasons)
                    {
                        if (!seasons.Contains(drinkSeason))
                            seasons.Add(drinkSeason);
                    }

                    break;
                case IngredientCategory.Crop:

                    var crop = await _cropService.GetCrop(ingredientId);
                    if (!seasons.Contains(crop.Season)) seasons.Add(crop.Season);

                    break;
                case IngredientCategory.Food:

                    var foodIngredients = await GetFoodIngredients(ingredientId);
                    var foodSeasons = new List<Season>();

                    foreach (var foodIngredient in foodIngredients)
                    {
                        var foodIngredientSeasons = await GetIngredientSeasons(
                            foodIngredient.Category, foodIngredient.IngredientId);

                        foreach (var foodIngredientSeason in foodIngredientSeasons)
                        {
                            if (!foodSeasons.Contains(foodIngredientSeason))
                                foodSeasons.Add(foodIngredientSeason);
                        }
                    }

                    foreach (var foodSeason in foodSeasons)
                    {
                        if (!seasons.Contains(foodSeason))
                            seasons.Add(foodSeason);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }

            return seasons;
        }
    }
}
