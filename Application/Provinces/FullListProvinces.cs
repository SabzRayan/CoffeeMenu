using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class FullListProvinces
    {
        public class Query : IRequest<Result<List<ProvinceDto>>> {}

        public class Handler : IRequestHandler<Query, Result<List<ProvinceDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<List<ProvinceDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var provinces = await context.Provinces
                    .OrderBy(a => a.Name)
                    .ProjectTo<ProvinceDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<ProvinceDto>>.Success(provinces);
            }
        }
    }
}
