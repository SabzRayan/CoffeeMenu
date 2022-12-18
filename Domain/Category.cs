using System;
using System.Collections.Generic;

namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid RestaurantId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Category Parent { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<Category> Children { get; set; } = new List<Category>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<BranchCategory> Branches { get; set; } = new List<BranchCategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
