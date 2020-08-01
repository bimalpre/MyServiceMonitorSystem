using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceMontorClass.Library
{
    public class Service
    {
        public event EventHandler ReportConnectionStatus;

        //List of Callers regisered for the service
        public List<Caller> callersList;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Port { get; set; }
        public string Host { get; set; }
        //Mili seconds
        private int pollingFrequency;
        public string Status { get; set; }

        public Service()
        {
            callersList = new List<Caller>();
        }
        public void DoCheckConnection(CancellationToken Token)
        {
            while (!Token.IsCancellationRequested)
            {
                //wait in mili seconds before checking connection
                Thread.Sleep(pollingFrequency);

                //
                //DO CHECK CONNECTION HERE
                //


                //This value hard-coded here, since connection check is not performed it always is "Ok"
                Status = "Ok";

                var eventraised = ReportConnectionStatus;
                if (eventraised != null)
                {
                    //return current instance
                    eventraised.Invoke(this, null);
                }
            }
            
        }

     

        public int PollingFrequency { get { return pollingFrequency; }
            set
            {
                //get the min frequency
                if (pollingFrequency > value)
                {
                    //check 1 second barrier
                    if (value > 1000)
                    {
                        pollingFrequency = value;
                    }
                    else
                    {
                        //1 second
                        pollingFrequency = 1000;
                    }
                }

            }
        }
        private ServiceOutage serviceOutage;
        public void CreateServiceOutage(ServiceOutage outage)
        {
            serviceOutage = outage;
        }

    }
}
