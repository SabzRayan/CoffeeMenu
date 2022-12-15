using System;
using System.Collections.Generic;

namespace Domain
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Logo { get; set; }
        public string HeaderPic { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public string Tweeter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public ICollection<RestaurantModule> Modules { get; set; } = new List<RestaurantModule>();
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<Category> Categories { get; set; } = new List<Category>(); 
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
