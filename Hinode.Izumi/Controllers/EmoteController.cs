using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.EmoteWebService;
using Hinode.Izumi.Services.WebServices.EmoteWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("emote")]
    [ApiController]
    public class EmoteController : ControllerBase
    {
        private readonly IEmoteWebService _emoteWebService;

        public EmoteController(IEmoteWebService emoteWebService)
        {
            _emoteWebService = emoteWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<EmoteWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _emoteWebService.GetAllEmotes());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(EmoteWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _emoteWebService.Get(long.Parse(id)));
        }

        [HttpPost, Route("/upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _emoteWebService.UploadEmotes();
            return Ok();
        }
    }
}
