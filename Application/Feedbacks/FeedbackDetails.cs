using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Feedbacks
{
    public class FeedbackDetails
    {
        public class Query : IRequest<Result<FeedbackDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<FeedbackDto>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<FeedbackDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var feedback = await context.Feedbacks
                    .ProjectTo<FeedbackDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);

                return Result<FeedbackDto>.Success(feedback);
            }
        }
    }
}
