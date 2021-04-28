using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.DrinkWebService;
using Hinode.Izumi.Services.WebServices.DrinkWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("drink")]
    [ApiController]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkWebService _drinkWebService;

        public DrinkController(IDrinkWebService drinkWebService)
        {
            _drinkWebService = drinkWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<DrinkWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _drinkWebService.GetAllDrinks());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(DrinkWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _drinkWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(DrinkWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, DrinkWebModel model)
        {
            model.Id = id;
            return Ok(await _drinkWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(DrinkWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(DrinkWebModel model)
        {
            return Ok(await _drinkWebService.Update(model));
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _drinkWebService.Remove(id);
            return Ok();
        }
    }
}
