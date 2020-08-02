using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMontorClass.Library
{
    public class ServiceOutage
    {
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
    }
}
