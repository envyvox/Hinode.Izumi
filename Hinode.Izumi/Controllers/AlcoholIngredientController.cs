using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService;
using Hinode.Izumi.Services.WebServices.AlcoholIngredientWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("alcohol-ingredient")]
    [ApiController]
    public class AlcoholIngredientController : ControllerBase
    {
        private readonly IAlcoholIngredientWebService _alcoholIngredientWebService;

        public AlcoholIngredientController(IAlcoholIngredientWebService alcoholIngredientWebService)
        {
            _alcoholIngredientWebService = alcoholIngredientWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<AlcoholIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _alcoholIngredientWebService.GetAllAlcoholIngredients());
        }

        [HttpGet, Route("ingredients/{id:long}")]
        [ProducesResponseType(typeof(IEnumerable<AlcoholIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(long id)
        {
            return Ok(await _alcoholIngredientWebService.GetAlcoholIngredients(id));
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _alcoholIngredientWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(AlcoholIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, AlcoholIngredientWebModel model)
        {
            model.Id = id;
            return Ok(await _alcoholIngredientWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(AlcoholIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AlcoholIngredientWebModel model)
        {
            return Ok(await _alcoholIngredientWebService.Update(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _alcoholIngredientWebService.Remove(id);
            return Ok();
        }
    }
}
