using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Users;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListUser.Query { Params = param }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpGet("myUsers")]
        public async Task<IActionResult> GetMyUsers([FromQuery] UserParams param)
        {
            return HandlePagedResult(await Mediator.Send(new MyUsers.Query { Params = param }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            return HandleResult(await Mediator.Send(new UserDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            return HandleResult(await Mediator.Send(new CreateUser.Command { User = user }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromRoute] string id, [FromBody] User user)
        {
            user.Id = id;
            return HandleResult(await Mediator.Send(new EditUser.Command { User = user }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            return HandleResult(await Mediator.Send(new DeleteUser.Command { Id = id }));
        }
    }
}
