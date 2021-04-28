using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.WebServices.CraftingWebService;
using Hinode.Izumi.Services.WebServices.CraftingWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("crafting")]
    [ApiController]
    public class CraftingController : ControllerBase
    {
        private readonly ICraftingWebService _craftingWebService;
        private readonly IIngredientService _ingredientService;
        private readonly ICalculationService _calc;

        public CraftingController(ICraftingWebService craftingWebService, IIngredientService ingredientService,
            ICalculationService calc)
        {
            _craftingWebService = craftingWebService;
            _ingredientService = ingredientService;
            _calc = calc;
        }

        /// <summary>
        /// Возвращает массив из всех изготавливаемых предметов.
        /// </summary>
        /// <returns>Массив из всех изготавливаемых предметов.</returns>
        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<CraftingWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            // получаем массив из всех изготавливаемых предметов
            var craftings = await _craftingWebService.GetAllCrafting();
            // добавляем к каждому изготавливаемому предмету его стоимости
            foreach (var crafting in craftings)
            {
                // считаем себестоимость
                crafting.CostPrice = await _ingredientService.GetCraftingCostPrice(crafting.Id);
                // считаем стоимость изготовления
                crafting.CraftingPrice = await _calc.CraftingPrice(crafting.CostPrice);
                // считаем цену нпс
                crafting.NpcPrice = await _calc.NpcPrice(MarketCategory.Crafting, crafting.CostPrice);
                // считаем чистую прибыль
                crafting.Profit = await _calc.Profit(crafting.NpcPrice, crafting.CostPrice, crafting.CraftingPrice);
            }

            // возвращаем дополненный массив из всех изготавливаемых предметов
            return Ok(craftings);
        }

        /// <summary>
        /// Возвращает изготавливаемый предмет с указанным id.
        /// </summary>
        /// <param name="id">Id изготавливаемого предмета.</param>
        /// <returns>Изготавливаемый предмет.</returns>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            // получаем изготавливаемый предмет
            var crafting = await _craftingWebService.Get(id);

            // считаем себестоимость
            crafting.CostPrice = await _ingredientService.GetCraftingCostPrice(crafting.Id);
            // считаем стоимость изготовления
            crafting.CraftingPrice = await _calc.CraftingPrice(crafting.CostPrice);
            // считаем цену нпс
            crafting.NpcPrice = await _calc.NpcPrice(MarketCategory.Crafting, crafting.CostPrice);
            // считаем чистую прибыль
            crafting.Profit = await _calc.Profit(crafting.NpcPrice, crafting.CostPrice, crafting.CraftingPrice);

            // возвращаем изготавливаемый предмет
            return Ok(crafting);
        }

        /// <summary>
        /// Изменяет изготавливаемый предмет.
        /// </summary>
        /// <param name="id">Id изготавливаемого предмета.</param>
        /// <param name="model">Модель изготавливаемого предмета.</param>
        /// <returns>Измененный изготавливаемый предмет.</returns>
        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, CraftingWebModel model)
        {
            // указываем что id изготавливаемого предмета это id полученный из роута
            model.Id = id;
            // обновляем в базе
            return Ok(await _craftingWebService.Update(model));
        }

        /// <summary>
        /// Добавляет новый изготавливаемый предмет.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Новый изготавливаемый предмет.</returns>
        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(CraftingWebModel model)
        {
            // добавляем изготавливаемый предмет в базу
            return Ok(await _craftingWebService.Update(model));
        }

        /// <summary>
        /// Удаляет изготавливаемый предмет.
        /// </summary>
        /// <param name="id">Id изготавливаемого предмета.</param>
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            // удаляем изготавливаемый предмет из базы
            await _craftingWebService.Remove(id);
            return Ok();
        }
    }
}
