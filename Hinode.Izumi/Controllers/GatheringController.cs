using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.GatheringWebService;
using Hinode.Izumi.Services.WebServices.GatheringWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("gathering")]
    [ApiController]
    public class GatheringController : ControllerBase
    {
        private readonly IGatheringWebService _gatheringWebService;

        public GatheringController(IGatheringWebService gatheringWebService)
        {
            _gatheringWebService = gatheringWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<GatheringWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _gatheringWebService.GetAllGathering());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(GatheringWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _gatheringWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(GatheringWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, GatheringWebModel model)
        {
            model.Id = id;
            return Ok(await _gatheringWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(GatheringWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(GatheringWebModel model)
        {
            return Ok(await _gatheringWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _gatheringWebService.Remove(id);
            return Ok();
        }
    }
}
