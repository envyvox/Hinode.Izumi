using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.AchievementWebService;
using Hinode.Izumi.Services.WebServices.AchievementWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("achievement")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementWebService _achievementWebService;

        public AchievementController(IAchievementWebService achievementWebService)
        {
            _achievementWebService = achievementWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<AchievementWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _achievementWebService.GetAllAchievements());
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(AchievementWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _achievementWebService.Get(id));
        }

        [HttpPost, Route("{id}")]
        [ProducesResponseType(typeof(AchievementWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, AchievementWebModel model)
        {
            model.Id = id;
            return Ok(await _achievementWebService.Update(model));
        }

        [HttpPost, Route("upload")]
        [ProducesResponseType(typeof(AchievementWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload()
        {
            await _achievementWebService.Upload();
            return Ok();
        }
    }
}
