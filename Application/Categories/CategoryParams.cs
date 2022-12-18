using Application.Core;
using System;

namespace Application.Categories
{
    public class CategoryParams : PagingParams
    {
        public Guid? ParentId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? RestaurantId { get; set; }
    }
}
