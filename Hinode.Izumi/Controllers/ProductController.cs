using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ProductWebService;
using Hinode.Izumi.Services.WebServices.ProductWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductWebService _productWebService;

        public ProductController(IProductWebService productWebService)
        {
            _productWebService = productWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<ProductWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _productWebService.GetAllProducts());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ProductWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _productWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(ProductWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, ProductWebModel model)
        {
            model.Id = id;
            return Ok(await _productWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(ProductWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(ProductWebModel model)
        {
            return Ok(await _productWebService.Update(model));
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _productWebService.Remove(id);
            return Ok();
        }
    }
}
