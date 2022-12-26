using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Provinces;

namespace API.Controllers
{
    public class ProvinceController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProvinces([FromQuery] PagingParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListProvinces.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetProvinces()
        {
            return HandleResult(await Mediator.Send(new FullListProvinces.Query()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProvince([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new ProvinceDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateProvince([FromBody] Province province)
        {
            return HandleResult(await Mediator.Send(new CreateProvince.Command { Name = province.Name }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProvince([FromRoute] Guid id, [FromBody] Province province)
        {
            return HandleResult(await Mediator.Send(new EditProvince.Command { ProvinceId = id, Name = province.Name }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvince([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteProvince.Command { Id = id }));
        }
    }
}
