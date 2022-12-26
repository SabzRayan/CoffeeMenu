using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Modules;

namespace API.Controllers
{
    public class ModuleController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetModules([FromQuery] ModuleParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListModules.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModule([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new ModuleDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] Module module)
        {
            return HandleResult(await Mediator.Send(new CreateModule.Command { Module = module }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditModule([FromRoute] Guid id, [FromBody] Module module)
        {
            module.Id = id;
            return HandleResult(await Mediator.Send(new EditModule.Command { Module = module }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteModule.Command { Id = id }));
        }
    }
}
