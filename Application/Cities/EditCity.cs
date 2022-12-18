using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cities
{
    public class EditCity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public City City { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.City).SetValidator(new CityValidator());
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
                var city = await context.Cities.FindAsync(new object[] { request.City.CityId }, cancellationToken);
                if (city == null) return null;
                city.Name = request.City.Name;
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update city");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
