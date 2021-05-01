using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.LocalizationWebService;
using Hinode.Izumi.Services.WebServices.LocalizationWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("localization")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationWebService _localizationWebService;

        public LocalizationController(ILocalizationWebService localizationWebService)
        {
            _localizationWebService = localizationWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<LocalizationWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _localizationWebService.GetAllLocalizations());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(LocalizationWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _localizationWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(LocalizationWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, LocalizationWebModel model)
        {
            model.Id = id;
            return Ok(await _localizationWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(LocalizationWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _localizationWebService.Upload();
            return Ok();
        }
    }
}
