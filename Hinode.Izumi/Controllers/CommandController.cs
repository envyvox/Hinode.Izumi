using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.CommandWebService;
using Hinode.Izumi.Services.WebServices.CommandWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("command")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandWebService _commandWebService;

        public CommandController(ICommandWebService commandWebService)
        {
            _commandWebService = commandWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<CommandInfo>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List() =>
            await Task.FromResult(
                Ok(_commandWebService.GetCommands()));
    }
}
