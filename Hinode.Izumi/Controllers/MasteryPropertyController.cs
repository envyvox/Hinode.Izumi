using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.MasteryPropertyWebService;
using Hinode.Izumi.Services.WebServices.MasteryPropertyWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("mastery-property")]
    [ApiController]
    public class MasteryPropertyController : ControllerBase
    {
        private readonly IMasteryPropertyWebService _masteryPropertyWebService;

        public MasteryPropertyController(IMasteryPropertyWebService masteryPropertyWebService)
        {
            _masteryPropertyWebService = masteryPropertyWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<MasteryPropertyWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _masteryPropertyWebService.GetAllProperties());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(MasteryPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _masteryPropertyWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(MasteryPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, MasteryPropertyWebModel model)
        {
            model.Id = id;
            return Ok(await _masteryPropertyWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(MasteryPropertyWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _masteryPropertyWebService.Upload();
            return Ok();
        }
    }
}
