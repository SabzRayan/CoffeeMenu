using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Branches;
using System.Collections.Generic;

namespace API.Controllers
{
    public class BranchController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBranches([FromQuery] BranchParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListBranch.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranch([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new BranchDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] Branch branch)
        {
            return HandleResult(await Mediator.Send(new CreateBranch.Command { Branch = branch }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPost("toggleCategories")]
        public async Task<IActionResult> AddBranchCategory([FromBody] List<BranchCategory> categories)
        {
            return HandleResult(await Mediator.Send(new AddBranchCategory.Command { BranchCategories = categories }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBranch([FromRoute] Guid id, [FromBody] Branch branch)
        {
            branch.Id = id;
            return HandleResult(await Mediator.Send(new EditBranch.Command { Branch = branch }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteBranch.Command { Id = id }));
        }
    }
}
