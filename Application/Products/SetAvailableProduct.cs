﻿using Application.Core;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class SetAvailableProduct
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public bool IsAvailable { get; set; }
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
                var product = await context.Products.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (product == null) return null;
                product.IsAvailable = request.IsAvailable;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to set availability for the product");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}