using Domain.Enum;
using System;

namespace Application.Feedbacks
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public Guid OrderDetailId { get; set; }
        public HappinessEnum Happiness { get; set; } = HappinessEnum.Normal;
        public string ReportText { get; set; }
        public string ProductName { get; set; }
    }
}
