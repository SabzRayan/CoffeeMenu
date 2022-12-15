using System;
using System.Collections.Generic;

namespace Domain
{
    public class Branch
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int TableCount { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Restaurant Restaurant { get; set; }
        public City City { get; set; }
        public Province Province { get; set; }
        public ICollection<BranchCategory> Categories { get; set; } = new List<BranchCategory>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
