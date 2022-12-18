using Application.Core;
using System;

namespace Application.Cities
{
    public class CityParams : PagingParams
    {
        public Guid ProvinceId { get; set; }
    }
}
