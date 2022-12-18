using Application.Core;
using Domain.Enum;
using System;

namespace Application.Orders
{
    public class OrderParams : PagingParams
    {
        public Guid? BranchId { get; set; }
        public OrderStatusEnum? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
