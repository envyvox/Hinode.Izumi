using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.GatheringPropertyWebService;
using Hinode.Izumi.Services.WebServices.GatheringPropertyWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("gathering-property")]
    [ApiController]
    public class GatheringPropertyController : ControllerBase
    {
        private readonly IGatheringPropertyWebService _gatheringPropertyWebService;

        public GatheringPropertyController(IGatheringPropertyWebService gatheringPropertyWebService)
        {
            _gatheringPropertyWebService = gatheringPropertyWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<GatheringPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _gatheringPropertyWebService.GetAllGatheringProperties());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(GatheringPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _gatheringPropertyWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(GatheringPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, GatheringPropertyWebModel model)
        {
            model.Id = id;
            return Ok(await _gatheringPropertyWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(GatheringPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _gatheringPropertyWebService.Upload();
            return Ok();
        }
    }
}
