﻿using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Restaurants
{
    public class RestaurantDetails
    {
        public class Query : IRequest<Result<RestaurantDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<RestaurantDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<RestaurantDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var restaurant = await context.Restaurants.Where(a => !a.IsDeleted)
                    .ProjectTo<RestaurantDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<RestaurantDto>.Success(restaurant);
            }
        }
    }
}
