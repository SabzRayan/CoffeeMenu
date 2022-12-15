using System;
using System.Collections.Generic;

namespace Domain
{
    public class Province
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    }
}
