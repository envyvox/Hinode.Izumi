using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.SeedWebService;
using Hinode.Izumi.Services.WebServices.SeedWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("seed")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ISeedWebService _seedWebService;

        public SeedController(ISeedWebService seedWebService)
        {
            _seedWebService = seedWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<SeedWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _seedWebService.GetAllSeeds());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(SeedWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _seedWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(SeedWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, SeedWebModel model)
        {
            model.Id = id;
            return Ok(await _seedWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(SeedWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(SeedWebModel model)
        {
            return Ok(await _seedWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _seedWebService.Remove(id);
            return Ok();
        }
    }
}
