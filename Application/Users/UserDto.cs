using Application.Branches;
using Application.Restaurants;
using System;
using System.Collections.Generic;

namespace Application.Users
{
    public class UserDto
    {
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Guid? RestaurantId { get; set; }
        public Guid? BranchId { get; set; }

        public RestaurantDto Restaurant { get; set; }
        public BranchDto Branch { get; set; }
        public IList<string> Roles { get; set; }
    }
}
