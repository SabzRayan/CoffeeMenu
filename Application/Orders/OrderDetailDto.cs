using Application.Products;
using System;

namespace Application.Orders
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public Double Price { get; set; }

        public ProductDto Product { get; set; }
    }
}
