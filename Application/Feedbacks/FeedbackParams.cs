using Application.Core;
using Domain.Enum;
using System;

namespace Application.Feedbacks
{
    public class FeedbackParams : PagingParams
    {
        public Guid? OrderId { get; set; }
        public Guid? ProductId { get; set;}
        public HappinessEnum? Happiness { get; set; }
        public Guid? BranchId { get; set; }
        public bool ShowDeleted { get; set; }
    }
}
