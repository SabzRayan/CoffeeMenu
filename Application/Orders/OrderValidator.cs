using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(c => c.BranchId).NotEmpty();
            RuleFor(c => c.TableNumber).GreaterThan(0);
            RuleFor(c => c.OrderDetails.Count).GreaterThan(0);
        }
    }
}
