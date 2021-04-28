using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.UserWebService;
using Hinode.Izumi.Services.WebServices.UserWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserWebService _userWebService;

        public UserController(IUserWebService userWebService)
        {
            _userWebService = userWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<UserWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _userWebService.GetAllUsers());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(UserWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _userWebService.Get(long.Parse(id)));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(UserWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] string id, UserWebModel model)
        {
            model.Id = id;
            return Ok(await _userWebService.Update(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(UserWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(UserWebModel model)
        {
            return Ok(await _userWebService.Update(model));
        }

        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            await _userWebService.Remove(long.Parse(id));
            return Ok();
        }
    }
}
