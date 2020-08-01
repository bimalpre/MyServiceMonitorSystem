using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMontorClass.Library
{
    class CallerServiceRegistry
    {
        public int registerId { get; set; }
        
        public Caller caller { get; set; }
        public Service service { get; set; }
    }
}
