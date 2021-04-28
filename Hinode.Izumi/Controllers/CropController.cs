using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CropWebService;
using Hinode.Izumi.Services.WebServices.CropWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("crop")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly ICropWebService _cropWebService;

        public CropController(ICropWebService cropWebService)
        {
            _cropWebService = cropWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<CropWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _cropWebService.GetAllCrops());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(CropWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _cropWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(CropWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, CropWebModel model)
        {
            model.Id = id;
            return Ok(await _cropWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(CropWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(CropWebModel model)
        {
            return Ok(await _cropWebService.Update(model));
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _cropWebService.Remove(id);
            return Ok();
        }
    }
}
