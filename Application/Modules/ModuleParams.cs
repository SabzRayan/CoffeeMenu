using Application.Core;
using Domain.Enum;
using System;

namespace Application.Modules
{
    public class ModuleParams : PagingParams
    {
        public Guid? RestaurantId { get; set; }
        public bool ShowDeleted { get; set; }
    }
}
