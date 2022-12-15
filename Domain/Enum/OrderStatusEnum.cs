using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum OrderStatusEnum
    {
        Received = 0,
        Failed = 1,
        Accepted = 2,
        Preparing = 3,
        OnTheWay = 4,
        Delivered = 5,
    }
}
