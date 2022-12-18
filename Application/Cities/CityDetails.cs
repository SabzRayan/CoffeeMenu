using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cities
{
    public class CityDetails
    {
        public class Query : IRequest<Result<CityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CityDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<CityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var city = await context.Cities
                    .ProjectTo<CityDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.CityId == request.Id, cancellationToken: cancellationToken);

                return Result<CityDto>.Success(city);
            }
        }
    }
}
