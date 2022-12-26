using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Cities;

namespace API.Controllers
{
    public class CityController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCities([FromQuery] CityParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListCities.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCities([FromQuery] CityParams param)
        {
            return HandleResult(await Mediator.Send(new FullListCities.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new CityDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] City city)
        {
            return HandleResult(await Mediator.Send(new CreateCity.Command { City = city }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCity([FromRoute] Guid id, [FromBody] City city)
        {
            city.CityId = id;
            return HandleResult(await Mediator.Send(new EditCity.Command { City = city }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteCity.Command { Id = id }));
        }
    }
}
