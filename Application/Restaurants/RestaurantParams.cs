using Application.Core;
using System;

namespace Application.Restaurants
{
    public class RestaurantParams : PagingParams
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string UserId { get; set; }
    }
}
