using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class EditOrder
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Order Order { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Order).SetValidator(new OrderValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await context.Orders.Include(a => a.OrderDetails).FirstOrDefaultAsync(a => a.Id == request.Order.Id, cancellationToken: cancellationToken);
                if (order == null) return null;
                order.Status = request.Order.Status;
                order.TableNumber = request.Order.TableNumber;
                if (order.OrderDetails.Count > 0)
                {
                    context.Remove(order.OrderDetails);
                    order.OrderDetails = request.Order.OrderDetails;
                }

                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update order");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
