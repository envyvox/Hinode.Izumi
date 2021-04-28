using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.FoodIngredientWebService;
using Hinode.Izumi.Services.WebServices.FoodIngredientWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("food-ingredient")]
    [ApiController]
    public class FoodIngredientController : ControllerBase
    {
        private readonly IFoodIngredientWebService _foodIngredientWebService;

        public FoodIngredientController(IFoodIngredientWebService foodIngredientWebService)
        {
            _foodIngredientWebService = foodIngredientWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<FoodIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _foodIngredientWebService.GetAllFoodIngredients());
        }

        [HttpGet, Route("ingredients/{id:long}")]
        [ProducesResponseType(typeof(IEnumerable<FoodIngredientWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(long id)
        {
            return Ok(await _foodIngredientWebService.GetFoodIngredients(id));
        }


        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _foodIngredientWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(FoodIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, FoodIngredientWebModel model)
        {
            model.Id = id;
            return Ok(await _foodIngredientWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(FoodIngredientWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(FoodIngredientWebModel model)
        {
            return Ok(await _foodIngredientWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _foodIngredientWebService.Remove(id);
            return Ok();
        }
    }
}
