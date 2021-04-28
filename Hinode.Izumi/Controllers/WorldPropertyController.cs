using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.WorldPropertyWebService;
using Hinode.Izumi.Services.WebServices.WorldPropertyWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("world-property")]
    [ApiController]
    public class WorldPropertyController : ControllerBase
    {
        private readonly IWorldPropertyWebService _worldPropertyWebService;

        public WorldPropertyController(IWorldPropertyWebService worldPropertyWebService)
        {
            _worldPropertyWebService = worldPropertyWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<WorldPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _worldPropertyWebService.GetAllProperties());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(WorldPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _worldPropertyWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(WorldPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, WorldPropertyWebModel model)
        {
            model.Id = id;
            return Ok(await _worldPropertyWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(WorldPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _worldPropertyWebService.Upload();
            return Ok();
        }
    }
}
