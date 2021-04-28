using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.FishWebService;
using Hinode.Izumi.Services.WebServices.FishWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("fish")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IFishWebService _fishWebService;

        public FishController(IFishWebService fishWebService)
        {
            _fishWebService = fishWebService;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<FishWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _fishWebService.GetAllFish());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(FishWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _fishWebService.Get(id));
        }

        [HttpPost]
        [Route("{id}")]
        [ProducesResponseType(typeof(FishWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, FishWebModel model)
        {
            model.Id = id;
            return Ok(await _fishWebService.Update(model));
        }

        [HttpPut]
        [Route("add")]
        [ProducesResponseType(typeof(FishWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(FishWebModel model)
        {
            return Ok(await _fishWebService.Update(model));
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _fishWebService.Remove(id);
            return Ok();
        }
    }
}
