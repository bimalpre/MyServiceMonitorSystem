using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMontorClass.Library
{
    public class MonitorServices
    {
       
        private Dictionary<string, Service> serviceList;//Host + Port
        private Dictionary<string, Caller> callerList;//Host + Port
        private Dictionary<string, CallerServiceRegistry> callerserviceList;//CallerName


        static void ReportedConnectionStatus(object svc, EventArgs e)
        {
            foreach(Caller caller in ((Service)svc).callersList)
            {
                Console.WriteLine("Caller : {0} Service : {1} Status : {2}", caller.CallerName, ((Service)svc).Host + ":" + ((Service)svc).Port, ((Service)svc).Status)
            }
            Console.WriteLine("The threshold was reached.");
        }
        public MonitorServices()
        {
            serviceList = new Dictionary<string, Service>();
            callerList = new Dictionary<string, Caller>();
            callerserviceList = new Dictionary<string, CallerServiceRegistry>();

        }
        public void RegisterService(string callername,string host,string port,int polling_frequency)
        {
         
            Caller caller = callerList[callername];
            //New caller
            if (caller == null)
            {
                caller = new Caller { CallerName = callername };
                callerList.Add(callername, caller );
            }
            Service service = serviceList[host + ":" + port];
            //New Service
            if (service == null)
            {
                service = new Service()
                {
                    Host = host,
                    Port = port
                };
                serviceList[host + ":" + port] = service;
            }
            //will set to min frequency with lowe limit of 1 second 
            service.PollingFrequency = polling_frequency;
            CallerServiceRegistry callerServiceRegistry = callerserviceList[callername];
            if(callerServiceRegistry == null)
            {
                callerserviceList[callername] = new CallerServiceRegistry()
                {
                    caller = caller,
                    service = service
                };
            }
            service.callersList.Add(caller);
            service.ReportConnectionStatus += ReportedConnectionStatus;
        }
    }
}
