using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BranchCategory
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BranchId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; }
        public Branch Branch { get; set; }
    }
}
