using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Restaurants;

namespace API.Controllers
{
    public class RestaurantController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantes([FromQuery] RestaurantParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListRestaurant.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new RestaurantDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            return HandleResult(await Mediator.Send(new CreateRestaurant.Command { Restaurant = restaurant }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditRestaurant([FromRoute] Guid id, [FromBody] Restaurant restaurant)
        {
            restaurant.Id = id;
            return HandleResult(await Mediator.Send(new EditRestaurant.Command { Restaurant = restaurant }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteRestaurant.Command { Id = id }));
        }
    }
}
