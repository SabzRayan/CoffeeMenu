using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Recipe { get; set; }
        public Double Price { get; set; }
        public Double Discount { get; set; }
        public string Tags { get; set; }
        public int Calory { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;
        public bool IsExist { get; set; } = true;
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public bool ChefSuggestion { get; set; } = false;

        public Category Category { get; set; }
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
