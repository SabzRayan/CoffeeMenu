using Domain;
using System;

namespace Application.Products
{
    public class ProductPriceDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
    }
}
