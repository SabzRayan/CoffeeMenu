using Application.Attachments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AttachmentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddAttachment.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteAttachment.Command { Id = id }));
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(Guid id)
        {
            return HandleResult(await Mediator.Send(new SetMainAttachment.Command { Id = id }));
        }
    }
}
