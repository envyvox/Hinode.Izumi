using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Services.WebServices.ContractWebService;
using Hinode.Izumi.Services.WebServices.ContractWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hinode.Izumi.Controllers
{
    [Route("contract")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractWebService _contractWebService;

        public ContractController(IContractWebService contractWebService)
        {
            _contractWebService = contractWebService;
        }

        [HttpGet, Route("list")]
        [ProducesResponseType(typeof(IEnumerable<ContractWebModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List()
        {
            return Ok(await _contractWebService.GetAllContracts());
        }

        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(typeof(ContractWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await _contractWebService.Get(id));
        }

        [HttpPost, Route("{id:long}")]
        [ProducesResponseType(typeof(ContractWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromRoute] long id, ContractWebModel model)
        {
            model.Id = id;
            return Ok(await _contractWebService.Upsert(model));
        }

        [HttpPut, Route("add")]
        [ProducesResponseType(typeof(ContractWebModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(ContractWebModel model)
        {
            return Ok(await _contractWebService.Upsert(model));
        }

        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromRoute] long id)
        {
            await _contractWebService.Remove(id);
            return Ok();
        }
    }
}
