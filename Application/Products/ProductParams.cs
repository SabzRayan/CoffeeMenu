using Application.Core;
using System;

namespace Application.Products
{
    public class ProductParams : PagingParams
    {
        public string Title { get; set; }
        public bool? WithDiscount { get; set; }
        public string Tag { get; set; }
        public int? MaxCalory { get; set; }
        public bool? ShowAvailable { get; set; }
        public bool? ShowExist { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? RestaurantId { get; set; }
    }
}
