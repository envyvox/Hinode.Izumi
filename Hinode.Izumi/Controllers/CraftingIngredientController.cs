using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CraftingIngredientWebService;
using Hinode.Izumi.Services.WebServices.CraftingIngredientWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("crafting-ingredient")]
    [ApiController]
    public class CraftingIngredientController : ControllerBase
    {
        private readonly ICraftingIngredientWebService _craftingIngredientWebService;

        public CraftingIngredientController(ICraftingIngredientWebService craftingIngredientWebService)
        {
            _craftingIngredientWebService = craftingIngredientWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<CraftingIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _craftingIngredientWebService.GetAllCraftingIngredients());
        }

        [HttpGet, Route("ingredients/{id:long}")]
        [ProducesResponseType(typeof(IEnumerable<CraftingIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(long id)
        {
            return Ok(await _craftingIngredientWebService.GetCraftingIngredients(id));
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _craftingIngredientWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, CraftingIngredientWebModel model)
        {
            model.Id = id;
            return Ok(await _craftingIngredientWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(CraftingIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(CraftingIngredientWebModel model)
        {
            return Ok(await _craftingIngredientWebService.Update(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _craftingIngredientWebService.Remove(id);
            return Ok();
        }
    }
}
