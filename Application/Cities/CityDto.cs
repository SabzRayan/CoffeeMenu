using System;

namespace Application.Cities
{
    public class CityDto
    {
        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
        public int? RestaurantCount { get; set; }
        public string ProvinceName { get; set; }
    }
}
