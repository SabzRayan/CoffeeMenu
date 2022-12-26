using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Orders;
using Domain.Enum;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        [Authorize(Policy = "IsManager")]
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListOrders.Query { Params = param }));
        }

        [Authorize(Policy = "IsCashier")]
        [HttpGet("newOrders")]
        public async Task<IActionResult> GetNewOrders([FromQuery] OrderParams param)
        {
            param.Status = OrderStatusEnum.Received;
            return HandlePagedResult(await Mediator.Send(new ListOrders.Query { Params = param }));
        }

        [Authorize(Policy = "IsCashier")]
        [HttpGet("acceptedOrders")]
        public async Task<IActionResult> GetAcceptedOrders([FromQuery] OrderParams param)
        {
            param.Status = OrderStatusEnum.Accepted;
            return HandlePagedResult(await Mediator.Send(new ListOrders.Query { Params = param }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new OrderDetails.Query { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            return HandleResult(await Mediator.Send(new CreateOrder.Command { Order = order }));
        }

        [Authorize(Policy = "IsWaiter")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrder([FromRoute] Guid id, [FromBody] Order order)
        {
            order.Id = id;
            return HandleResult(await Mediator.Send(new EditOrder.Command { Order = order }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteOrder.Command { Id = id }));
        }
    }
}
