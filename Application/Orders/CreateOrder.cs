﻿using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class CreateOrder
    {
        public class Command : IRequest<Result<OrderDto>>
        {
            public Order Order { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.Order).SetValidator(new OrderValidator());
            }
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
                await context.Orders.AddAsync(request.Order, cancellationToken);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<OrderDto>.Failure("Failed to create order");
                return Result<OrderDto>.Success(mapper.Map<OrderDto>(request.Order));
            }
        }
    }

}
