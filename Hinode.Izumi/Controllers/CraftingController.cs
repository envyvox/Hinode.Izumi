using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.AlcoholService.Queries;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.CraftingService.Queries;
using Hinode.Izumi.Services.WebServices.CraftingWebService;
using Hinode.Izumi.Services.WebServices.CraftingWebService.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("crafting")]
    [ApiController]
    public class CraftingController : ControllerBase
    {
        private readonly ICraftingWebService _craftingWebService;
        private readonly IMediator _mediator;

        public CraftingController(ICraftingWebService craftingWebService, IMediator mediator)
        {
            _craftingWebService = craftingWebService;
            _mediator = mediator;
        }

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
                crafting.CostPrice = await _mediator.Send(new GetAlcoholCostPriceQuery(
                    crafting.Id));
                // считаем стоимость изготовления
                crafting.CraftingPrice = await _mediator.Send(new GetCraftingPriceQuery(
                    crafting.CostPrice));
                // считаем цену нпс
                crafting.NpcPrice = await _mediator.Send(new GetNpcPriceQuery(
                    MarketCategory.Crafting, crafting.CostPrice));
                // считаем чистую прибыль
                crafting.Profit = await _mediator.Send(new GetProfitQuery(
                    crafting.CostPrice, crafting.CraftingPrice, crafting.NpcPrice));
            }

            // возвращаем дополненный массив из всех изготавливаемых предметов
            return Ok(craftings);
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            // получаем изготавливаемый предмет
            var crafting = await _craftingWebService.Get(id);

            // считаем себестоимость
            crafting.CostPrice = await _mediator.Send(new GetAlcoholCostPriceQuery(
                crafting.Id));
            // считаем стоимость изготовления
            crafting.CraftingPrice = await _mediator.Send(new GetCraftingPriceQuery(
                crafting.CostPrice));
            // считаем цену нпс
            crafting.NpcPrice = await _mediator.Send(new GetNpcPriceQuery(
                MarketCategory.Crafting, crafting.CostPrice));
            // считаем чистую прибыль
            crafting.Profit = await _mediator.Send(new GetProfitQuery(
                crafting.CostPrice, crafting.CraftingPrice, crafting.NpcPrice));

            // возвращаем изготавливаемый предмет
            return Ok(crafting);
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, CraftingWebModel model)
        {
            model.Id = id;
            return Ok(await _craftingWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(CraftingWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(CraftingWebModel model)
        {
            return Ok(await _craftingWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _craftingWebService.Remove(id);
            return Ok();
        }
    }
}
