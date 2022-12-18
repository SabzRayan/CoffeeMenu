using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Categories;
using System.Reflection.Metadata;
using Domain.Constant;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListCategory.Query { CategoryParams= param }));
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories([FromQuery] CategoryParams param)
        {
            return HandleResult(await Mediator.Send(new FullListCategory.Query { CategoryParams = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new CategoryDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            return HandleResult(await Mediator.Send(new CreateCategory.Command { Category = category }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, [FromBody] Category category)
        {
            category.Id = id;
            return HandleResult(await Mediator.Send(new EditCategory.Command { Category = category }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteCategory.Command { Id = id }));
        }
    }
}
