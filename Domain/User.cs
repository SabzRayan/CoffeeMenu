using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class User : IdentityUser
    {
        public Guid? RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? BranchId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public RoleEnum Role { get; set; }

        public Restaurant Restaurant { get; set; }
        public Branch Branch { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
