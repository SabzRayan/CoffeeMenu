using Application.Core;
using System;

namespace Application.Branches
{
    public class BranchParams : PagingParams
    {
        public string Name { get; set; }
        public Double? Lat { get; set; }
        public Double? Lng { get; set; }
        public Guid? CityId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? RestaurantId { get; set; }
    }
}
