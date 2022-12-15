using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RestaurantModule
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddYears(1);
        public Double Paid { get; set; } = 0;

        public Restaurant Restaurant { get; set; }
        public Module Module { get; set; }
    }
}
