﻿using Application.Core;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class DeleteProvince
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var province = await context.Provinces.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
                if (province == null) return null;
                context.Remove(province);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the province");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
