using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class ProvinceDetails
    {
        public class Query : IRequest<Result<ProvinceDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ProvinceDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<ProvinceDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var city = await context.Provinces
                    .ProjectTo<ProvinceDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.ProvinceId == request.Id, cancellationToken);

                return Result<ProvinceDto>.Success(city);
            }
        }
    }
}
