using Application.Core;
using AutoMapper;
using Domain;
using Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class ChangeOrderStatus
    {
        public class Command : IRequest<Result<OrderDto>>
        {
            public Guid OrderId { get; set; }
            public OrderStatusEnum Status { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<OrderDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<OrderDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await context.Orders.Include(a => a.OrderDetails).FirstOrDefaultAsync(a => a.Id == request.OrderId, cancellationToken: cancellationToken);
                if (order == null) return null;
                order.Status = request.Status;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<OrderDto>.Failure("Failed to update order");
                return Result<OrderDto>.Success(mapper.Map<OrderDto>(order));
            }
        }
    }
}
