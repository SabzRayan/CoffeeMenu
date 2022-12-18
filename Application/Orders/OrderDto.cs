using Domain.Enum;
using Domain;
using System;
using System.Collections.Generic;

namespace Application.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public int TableNumber { get; set; }
        public OrderStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Branch Branch { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
