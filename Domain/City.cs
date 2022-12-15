using System;
using System.Collections.Generic;

namespace Domain
{
    public class City
    {
        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Province Province { get; set; }
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
