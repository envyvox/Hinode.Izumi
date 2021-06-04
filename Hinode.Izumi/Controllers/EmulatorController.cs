using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.EmulatorWebService.Models;
using Hinode.Izumi.Services.WebServices.EmulatorWebService.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("emulator")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmulatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("fishing")]
        [ProducesResponseType(typeof(FishingResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> EmulateFishing(FishingEmulateSetup setup)
        {
            return Ok(await _mediator.Send(new GetFishingEmulateResultQuery(setup)));
        }

        [HttpPost, Route("explore")]
        [ProducesResponseType(typeof(ExploreResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> EmulateExplore(ExploreEmulateSetup setup)
        {
            return Ok(await _mediator.Send(new GetExploreEmulateResultQuery(setup)));
        }
    }
}
