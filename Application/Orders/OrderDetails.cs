using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderDetails
    {
        public class Query : IRequest<Result<OrderDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<OrderDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await context.Orders.Where(a => !a.IsDeleted)
                    .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<OrderDto>.Success(order);
            }
        }
    }
}
