using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.WebServices.FoodWebService;
using Hinode.Izumi.Services.WebServices.FoodWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodWebService _foodWebService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;
        private readonly IEmoteService _emoteService;

        public FoodController(IFoodWebService foodWebService, ICalculationService calc,
            IIngredientService ingredientService, IEmoteService emoteService)
        {
            _foodWebService = foodWebService;
            _calc = calc;
            _ingredientService = ingredientService;
            _emoteService = emoteService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<FoodWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            // получаем иконки из базы
            var emotes = await _emoteService.GetEmotes();
            // получаем массив из всех блюд
            var foods = await _foodWebService.GetAllFood();
            // добавляем к каждому блюду его стоимости
            foreach (var food in foods)
            {
                // считаем себестоимость
                food.CostPrice = await _ingredientService.GetFoodCostPrice(food.Id);
                // считаем стоимость изготовления
                food.CookingPrice = await _calc.CraftingPrice(food.CostPrice);
                // считаем цену нпс
                food.NpcPrice = await _calc.NpcPrice(MarketCategory.Food, food.CostPrice);
                // считаем чистую прибыль
                food.Profit = await _calc.Profit(food.NpcPrice, food.CostPrice, food.CookingPrice);
                // считаем стоимость рецепта
                food.RecipePrice = await _calc.FoodRecipePrice(food.CostPrice);
                // считаем количество восстанавливаемой энергии
                food.Energy = await _calc.FoodEnergyRecharge(food.CostPrice, food.CookingPrice);
                // получаем сезоны блюда
                food.Seasons = await _ingredientService.GetFoodSeasons(food.Id);
                // получаем иконку блюда
                food.EmoteId = emotes.GetEmoteIdOrBlank(food.Name);
            }

            // возвращаем дополненный массив из всех блюд
            return Ok(foods);
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            // получаем блюдо
            var food = await _foodWebService.Get(id);

            // считаем себестоимость
            food.CostPrice = await _ingredientService.GetFoodCostPrice(food.Id);
            // считаем стоимость изготовления
            food.CookingPrice = await _calc.CraftingPrice(food.CostPrice);
            // считаем цену нпс
            food.NpcPrice = await _calc.NpcPrice(MarketCategory.Food, food.CostPrice);
            // считаем чистую прибыль
            food.Profit = await _calc.Profit(food.NpcPrice, food.CostPrice, food.CookingPrice);
            // считаем стоимость рецепта
            food.RecipePrice = await _calc.FoodRecipePrice(food.CostPrice);
            // считаем количество восстанавливаемой энергии
            food.Energy = await _calc.FoodEnergyRecharge(food.CostPrice, food.CookingPrice);
            // получаем сезоны блюда
            food.Seasons = await _ingredientService.GetFoodSeasons(food.Id);

            // возвращаем блюдо
            return Ok(food);
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, FoodWebModel model)
        {
            model.Id = id;
            return Ok(await _foodWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(FoodWebModel model)
        {
            return Ok(await _foodWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _foodWebService.Remove(id);
            return Ok();
        }
    }
}
