using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(c => c.Title).NotEmpty();
            RuleFor(c => c.ProductPrices.Count).GreaterThan(0);
            RuleFor(c => c.ProductPrices.First().Price).GreaterThan(0);
            RuleFor(c => c.CategoryId).NotEmpty();
            RuleFor(c => c.Attachments.Count).GreaterThan(0);
            RuleFor(c => c.Attachments.Count(a => a.IsMain)).Equal(1).OverridePropertyName("Main Attachment Count");
        }
    }
}
