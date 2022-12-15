using System;
using System.Collections.Generic;

namespace Domain
{
    public class Module
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public ICollection<RestaurantModule> Restaurants { get; set; } = new List<RestaurantModule>();
    }
}
