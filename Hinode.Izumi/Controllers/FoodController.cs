using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
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

        public FoodController(IFoodWebService foodWebService, ICalculationService calc,
            IIngredientService ingredientService)
        {
            _foodWebService = foodWebService;
            _calc = calc;
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Возвращает массив из всех блюд.
        /// </summary>
        /// <returns>Массив из всех блюд.</returns>
        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<FoodWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            // получаем массив из всех блюд
            var foods = await _foodWebService.GetAllFood();
            // добавляем к каждому блюду его стоимости
            foreach (var food in foods)
            {
                // считаем себестоимость
                food.CostPrice = await _ingredientService.GetFoodCostPrice(food.Id);
                // считаем стоимость изготовления
                food.CraftingPrice = await _calc.CraftingPrice(food.CostPrice);
                // считаем цену нпс
                food.NpcPrice = await _calc.NpcPrice(MarketCategory.Food, food.CostPrice);
                // считаем чистую прибыль
                food.Profit = await _calc.Profit(food.NpcPrice, food.CostPrice, food.CraftingPrice);
                // считаем стоимость рецепта
                food.RecipePrice = await _calc.FoodRecipePrice(food.CostPrice);
            }

            // возвращаем дополненный массив из всех блюд
            return Ok(foods);
        }

        /// <summary>
        /// Возвращает блюдо с указанным id.
        /// </summary>
        /// <param name="id">Id блюда.</param>
        /// <returns>Блюдо.</returns>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            // получаем блюдо
            var food = await _foodWebService.Get(id);

            // считаем себестоимость
            food.CostPrice = await _ingredientService.GetFoodCostPrice(food.Id);
            // считаем стоимость изготовления
            food.CraftingPrice = await _calc.CraftingPrice(food.CostPrice);
            // считаем цену нпс
            food.NpcPrice = await _calc.NpcPrice(MarketCategory.Food, food.CostPrice);
            // считаем чистую прибыль
            food.Profit = await _calc.Profit(food.NpcPrice, food.CostPrice, food.CraftingPrice);
            // считаем стоимость рецепта
            food.RecipePrice = await _calc.FoodRecipePrice(food.CostPrice);

            // возвращаем блюдо
            return Ok(food);
        }

        /// <summary>
        /// Изменяет блюдо.
        /// </summary>
        /// <param name="id">Id блюда.</param>
        /// <param name="model">Модель блюда.</param>
        /// <returns>Измененное блюдо.</returns>
        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, FoodWebModel model)
        {
            // указываем что id блюда это id полученный из роута
            model.Id = id;
            // обновляем в базе
            return Ok(await _foodWebService.Update(model));
        }

        /// <summary>
        /// Добавляет новое блюдо.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Новое блюдо.</returns>
        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(FoodWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(FoodWebModel model)
        {
            // добавляем блюдо в базу
            return Ok(await _foodWebService.Update(model));
        }

        /// <summary>
        /// Удаляет блюдо.
        /// </summary>
        /// <param name="id">Id блюда.</param>
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            // удаляем блюло из базы
            await _foodWebService.Remove(id);
            return Ok();
        }
    }
}
