using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class ListProduct
    {
        public class Query : IRequest<Result<PagedList<ProductDto>>>
        {
            public ProductParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ProductDto>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<Result<PagedList<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Products
                    .Where(a => !a.IsDeleted &&
                                (string.IsNullOrEmpty(request.Params.Title) || a.Title.Contains(request.Params.Title)) &&
                                //(request.Params.WithDiscount == null || (request.Params.WithDiscount == true ? a.Price > a.Discount : (a.Discount <= a.Price || a.Discount == 0))) &&
                                (string.IsNullOrEmpty(request.Params.Tag) || a.Tags.Contains(request.Params.Tag)) &&
                                (request.Params.MaxCalory == null || a.Calory <= request.Params.MaxCalory) &&
                                (request.Params.ShowAvailable == null || request.Params.ShowAvailable == false || a.IsAvailable) &&
                                (request.Params.ShowExist== null || request.Params.ShowExist == false || a.IsExist) &&
                                (request.Params.CategoryId == null || a.CategoryId == request.Params.CategoryId))
                    .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                    .AsQueryable();

                return Result<PagedList<ProductDto>>.Success(
                    await PagedList<ProductDto>.CreateAsync(
                        query,
                        request.Params.PageNumber,
                        request.Params.PageSize)
                );
            }
        }
    }
}
