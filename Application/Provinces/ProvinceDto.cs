using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Provinces
{
    public class ProvinceDto
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
        public int? RestaurantCount { get; set; }
    }
}
