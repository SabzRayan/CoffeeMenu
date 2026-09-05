using Application.Categories;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Branches
{
    public class BranchCategoryDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BranchId { get; set; }

        public CategoryDto Category { get; set; }
        public BranchDto Branch { get; set; }
    }
}
