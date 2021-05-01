using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CraftingPropertyWebService;
using Hinode.Izumi.Services.WebServices.CraftingPropertyWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("crafting-property")]
    [ApiController]
    public class CraftingPropertyController : ControllerBase
    {
        private readonly ICraftingPropertyWebService _craftingPropertyWebService;

        public CraftingPropertyController(ICraftingPropertyWebService craftingPropertyWebService)
        {
            _craftingPropertyWebService = craftingPropertyWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<CraftingPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _craftingPropertyWebService.GetAllCraftingProperties());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _craftingPropertyWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(CraftingPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, CraftingPropertyWebModel model)
        {
            model.Id = id;
            return Ok(await _craftingPropertyWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(CraftingPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _craftingPropertyWebService.Upload();
            return Ok();
        }
    }
}
