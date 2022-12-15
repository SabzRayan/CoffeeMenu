using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public int TableNumber { get; set; }
        public OrderStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public Branch Branch { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
