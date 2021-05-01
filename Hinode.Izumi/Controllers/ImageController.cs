using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ImageWebService;
using Hinode.Izumi.Services.WebServices.ImageWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageWebService _imageWebService;

        public ImageController(IImageWebService imageWebService)
        {
            _imageWebService = imageWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<ImageWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _imageWebService.GetAllImages());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(ImageWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _imageWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(ImageWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, ImageWebModel model)
        {
            model.Id = id;
            return Ok(await _imageWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(ImageWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _imageWebService.Upload();
            return Ok();
        }
    }
}
