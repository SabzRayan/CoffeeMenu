using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Feedbacks;

namespace API.Controllers
{
    public class FeedbackController : BaseApiController
    {
        [Authorize(Policy = "IsManager")]
        [HttpGet]
        public async Task<IActionResult> GetFeedbacks([FromQuery] FeedbackParams param)
        {
            param.ShowDeleted = false;
            return HandlePagedResult(await Mediator.Send(new ListFeedbacks.Query { Params = param }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedback([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new FeedbackDetails.Query { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] Feedback feedback)
        {
            return HandleResult(await Mediator.Send(new CreateFeedback.Command { Feedback = feedback }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteFeedback.Command { Id = id }));
        }
    }
}
