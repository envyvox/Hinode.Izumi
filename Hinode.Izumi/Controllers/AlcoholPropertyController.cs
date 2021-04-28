using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService;
using Hinode.Izumi.Services.WebServices.AlcoholPropertyWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("alcohol-property")]
    [ApiController]
    public class AlcoholPropertyController : ControllerBase
    {
        private readonly IAlcoholPropertyWebService _alcoholPropertyWebService;

        public AlcoholPropertyController(IAlcoholPropertyWebService alcoholPropertyWebService)
        {
            _alcoholPropertyWebService = alcoholPropertyWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<AlcoholPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _alcoholPropertyWebService.GetAllAlcoholProperties());
        }

        [HttpGet, Route("list/{id:long}")]
        [ProducesResponseType(typeof(IEnumerable<AlcoholPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(long id)
        {
            return Ok(await _alcoholPropertyWebService.GetAlcoholProperties(id));
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _alcoholPropertyWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, AlcoholPropertyWebModel model)
        {
            model.Id = id;
            return Ok(await _alcoholPropertyWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(AlcoholPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _alcoholPropertyWebService.Upload();
            return Ok();
        }
    }
}
