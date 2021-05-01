using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.RpgServices.CalculationService;
using Hinode.Izumi.Services.RpgServices.IngredientService;
using Hinode.Izumi.Services.WebServices.AlcoholWebService;
using Hinode.Izumi.Services.WebServices.AlcoholWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("alcohol")]
    [ApiController]
    public class AlcoholController : ControllerBase
    {
        private readonly IAlcoholWebService _alcoholWebService;
        private readonly ICalculationService _calc;
        private readonly IIngredientService _ingredientService;

        public AlcoholController(IAlcoholWebService alcoholWebService, ICalculationService calc,
            IIngredientService ingredientService)
        {
            _alcoholWebService = alcoholWebService;
            _calc = calc;
            _ingredientService = ingredientService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<AlcoholWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            // получаем массив из всего алкоголя
            var alcohols = await _alcoholWebService.GetAllAlcohols();
            // добавляем к каждому алкоголю его стоимости
            foreach (var alcohol in alcohols)
            {
                // считаем себестоимость
                alcohol.CostPrice = await _ingredientService.GetAlcoholCostPrice(alcohol.Id);
                // считаем стоимость изготовления
                alcohol.CraftingPrice = await _calc.CraftingPrice(alcohol.CostPrice);
                // считаем цену нпс
                alcohol.NpcPrice = await _calc.NpcPrice(MarketCategory.Alcohol, alcohol.CostPrice);
                // считаем чистую прибыль
                alcohol.Profit = await _calc.Profit(alcohol.NpcPrice, alcohol.CostPrice, alcohol.CraftingPrice);
            }

            // возвращаем дополненный массив из всего алкоголя
            return Ok(alcohols);
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            // получаем алкоголь
            var alcohol = await _alcoholWebService.Get(id);

            // считаем себестоимость
            alcohol.CostPrice = await _ingredientService.GetAlcoholCostPrice(alcohol.Id);
            // считаем стоимость изготовления
            alcohol.CraftingPrice = await _calc.CraftingPrice(alcohol.CostPrice);
            // считаем цену нпс
            alcohol.NpcPrice = await _calc.NpcPrice(MarketCategory.Alcohol, alcohol.CostPrice);
            // считаем чистую прибыль
            alcohol.Profit = await _calc.Profit(alcohol.NpcPrice, alcohol.CostPrice, alcohol.CraftingPrice);

            // возвращаем алкоголь
            return Ok(alcohol);
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, AlcoholWebModel model)
        {
            model.Id = id;
            return Ok(await _alcoholWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(AlcoholWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AlcoholWebModel model)
        {
            return Ok(await _alcoholWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _alcoholWebService.Remove(id);
            return Ok();
        }
    }
}
