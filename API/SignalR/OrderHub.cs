using Application.Orders;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class OrderHub : Hub
    {
        private readonly IMediator mediator;

        public OrderHub(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task PlaceOrder(CreateOrder.Command command)
        {
            var order = await mediator.Send(command);
            await Clients.Group("Cashier")
                    .SendAsync("ReceiveOrder", order.Value);
        }

        public async Task ChangeStatus(ChangeOrderStatus.Command command)
        {
            var order = await mediator.Send(command);
            await Clients.Group(command.OrderId.ToString()).SendAsync("ChangeOrderStatus", order.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auth = httpContext.Request.Headers.Authorization;
            var orderId = httpContext.Request.Query["orderId"];
            if (orderId.Any())
                await Groups.AddToGroupAsync(Context.ConnectionId, orderId);
        }
    }
}
