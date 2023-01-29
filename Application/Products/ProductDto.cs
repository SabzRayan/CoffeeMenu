using Application.Attachments;
using Domain;
using System;
using System.Collections.Generic;

namespace Application.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Recipe { get; set; }
        public Double Price { get; set; }
        //public Double Discount { get; set; }
        public string Tags { get; set; }
        public int Calory { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsExist { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
        public int LikeCount { get; set; }
        public bool ChefSuggestion { get; set; }

        public IEnumerable<AttachmentDto> Attachments { get; set; }
        public IEnumerable<ProductPriceDto> ProductPrices { get; set; }
    }
}
