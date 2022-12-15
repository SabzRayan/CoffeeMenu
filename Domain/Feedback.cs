using Domain.Enum;
using System;

namespace Domain
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid OrderDetailId { get; set; }
        public HappinessEnum Happiness { get; set; } = HappinessEnum.Normal;
        public string ReportText { get; set; }
        public bool IsDeleted { get; set; } = false;

        public OrderDetail OrderDetail { get; set; }
    }
}
