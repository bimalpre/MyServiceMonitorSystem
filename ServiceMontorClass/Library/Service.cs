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
        public event EventHandler ReportedConnectionStatus;

        public MonitorServices monitorServices;
        //List of Callers regisered for the service
        public List<Caller> callersList;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Port { get; set; }
        public string Host { get; set; }
        //Mili seconds
        public int pollingFrequency;
        public ConnectionStatus Status { get; set; }

        public Service()
        {
            callersList = new List<Caller>();
            serviceOutage = null;
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
                        //1 secondC:\code\ServiceMontorClass\ServiceMontorClass\App.config
                        pollingFrequency = 1000;
                    }
                }
                else
                {
                    pollingFrequency = value;
                }

            }
        }
        bool IsOutage = false;
        public void DoCheckConnection(CancellationToken Token)
        {
              


            //
            //DO PERFORM SERVICE OUTAGE
            //
            if(serviceOutage != null)
            {
                if( TimeBetween(DateTime.Now, serviceOutage.startTime, serviceOutage.endTime))
                {
                    IsOutage = true;
                    return;
                }
            }
            if (IsOutage)
            {
                IsOutage = false;
                serviceOutage = null;
            }

            //wait in mili seconds before checking connection
            Thread.Sleep(this.pollingFrequency);

            //
            //DO CHECK CONNECTION HERE
            //

            Random random = new Random();

            //This value hard-coded here, since connection check is not performed
            this.Status = ConnectionStatus.Connection_Up;
            if (random.Next(0, 1000) > 900)
                this.Status = ConnectionStatus.Connection_Down;

            var eventraised = ReportedConnectionStatus;
            if (eventraised != null)
            {
                //return current instance
                eventraised.Invoke(this, null);
            }




        }


        private ServiceOutage serviceOutage;
        public void CreateServiceOutage(ServiceOutage outage)
        {
            serviceOutage = outage;
        }

        bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

    }
}



