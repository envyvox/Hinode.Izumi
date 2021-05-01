using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.TransitWebService;
using Hinode.Izumi.Services.WebServices.TransitWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("transit")]
    [ApiController]
    public class TransitController : ControllerBase
    {
        private readonly ITransitWebService _transitWebService;

        public TransitController(ITransitWebService transitWebService)
        {
            _transitWebService = transitWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<TransitWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _transitWebService.GetAllTransits());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(TransitWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _transitWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(TransitWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, TransitWebModel model)
        {
            model.Id = id;
            return Ok(await _transitWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(TransitWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(TransitWebModel model)
        {
            return Ok(await _transitWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _transitWebService.Remove(id);
            return Ok();
        }
    }
}
