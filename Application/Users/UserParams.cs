using Application.Core;
using Domain.Enum;
using System;

namespace Application.Users
{
    public class UserParams : PagingParams
    {
        public string Name { get; set; }
        public Guid? RestaurantId { get; set; }
        public Guid? BranchId { get; set; }
        public RoleEnum? Role { get; set; }
    }
}
