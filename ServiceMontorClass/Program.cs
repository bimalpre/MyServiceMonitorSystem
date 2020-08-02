using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceMontorClass.Library;

namespace ServiceMontorClass
{
    class Program
    {
        static void Main(string[] args1)
        {
            create();
            Console.WriteLine("Over");
            Console.ReadLine();
        }
      
        static void create()
        {
            MonitorServices monitorServices = new MonitorServices();
            Thread t0 = new Thread(() => monitorServices.DoRegisterService("Caller 1", "abccOutage_111.com", "2300", 1000, 1000));
            t0.Start();
            Thread t1 = new Thread(() => monitorServices.DoRegisterService("Caller 1", "google.com.com", "2300", 5000, 1000));
            t1.Start();
            Thread t2 = new Thread(() => monitorServices.DoCheckConnection(new CancellationToken()));
            t2.Start();
            Thread t3 = new Thread(() => monitorServices.DoRegisterService("Caller 2", "yahoo.com", "1300", 3000, 4000));
            t3.Start();
            Thread t4 = new Thread(() => monitorServices.DoRegisterService("Caller 2", "abcf0000000001.com", "5300", 7000, 5000));
            t4.Start();
            //
            ServiceOutage serviceOutage = new ServiceOutage()
            {
                startTime = DateTime.Now.AddSeconds(15).TimeOfDay,
                endTime = DateTime.Now.AddSeconds(900).TimeOfDay

            };
            Thread t7 = new Thread(() => monitorServices.DoCreateServiceOutage("abccOutage_111.com", "2300", serviceOutage));
            t7.Start();


            Thread t5 = new Thread(() => monitorServices.DoRegisterService("Caller 1", "ikaman.lk", "4300", 5000, 5000));
            t5.Start();
            Thread t6 = new Thread(() => monitorServices.DoRegisterService("Caller 3", "ikaman.lk", "4300", 2000, 5000));
            t6.Start();
            Thread t8 = new Thread(() => monitorServices.DoRegisterService("Caller 3", "ikaman.lk", "4900", 1000, 1000));
            t8.Start();


        }
    }
}
