using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ProductPrice
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Double Price { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
