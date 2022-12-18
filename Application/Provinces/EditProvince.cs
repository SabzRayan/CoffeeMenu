using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class EditProvince
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ProvinceId { get; set; }
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.ProvinceId).NotEmpty();
                RuleFor(a => a.Name).NotEmpty();
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
                var province = await context.Provinces.FindAsync(new object[] { request.ProvinceId }, cancellationToken);
                if (province == null) return null;
                province.Name = request.Name;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update province");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
