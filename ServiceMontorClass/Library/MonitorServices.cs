﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceMontorClass.Library
{
    public class MonitorServices
    {
        public event EventHandler ReportConnectionStatusAfterGracePeriod;
        public event EventHandler ReportNewRegister;
        public event EventHandler ServiceOutage;
        public Dictionary<string, Service> serviceList;//Host + Port


        void ReportedConnectionStatus(object svc, EventArgs e)
        {
            foreach(Caller caller in ((Service)svc).callersList)
            {
                ReportConnectionStatusAfterGracePeriod += InformStaus;
                var Inform = ReportConnectionStatusAfterGracePeriod;
                if(Inform != null)
                {
                    Inform.Invoke(svc, null);
                }
               

            }

        }
        void InformStaus(object svc, EventArgs e)
        {
            foreach (Caller caller in ((Service)svc).callersList)
            {
                var eventraised = ReportConnectionStatusAfterGracePeriod;
                if (eventraised != null)
                {
                    if(((Service)svc).Status == ConnectionStatus.Connection_Down)
                        Thread.Sleep(caller.GracePeriod);
                    //Grace period Greater
                    if (((Service)svc).PollingFrequency < caller.GracePeriod)
                    {
                        Console.WriteLine("Caller : {0} Service : {1} Status : {2}", caller.CallerName, ((Service)svc).Host + ":" + ((Service)svc).Port, ((((Service)svc).Status == ConnectionStatus.Connection_Up)? "Connection Up":"Connection Down"));
                    }
                    else
                    {
                        //perform Extra Checks
                    }
                }

            }

        }
        void RegisterNewCallerService(object o, EventArgs e)
        {
            
        }

        void RegisterService(object o, EventArgs e)
        {
            var Inform = ReportNewRegister;
            if (Inform != null)
            {
                Inform.Invoke(o, null);
            }

        }
        public MonitorServices()
        {
            serviceList = new Dictionary<string, Service>();

        }
        public bool DoRegisterService(string callername,string host,string port,int polling_frequency,int GraceTime)
        {

            List<Service> sl = new List<Service>();
            Caller caller = null;//callerList.FirstOrDefault( r => r.Key == callername).Value;
            //New caller
            if (caller == null)
            {
                caller = new Caller { CallerName = callername,
                    GracePeriod = GraceTime
                };
                //callerList.Add(callername, caller );
            }
            Service service = null;
            //New Service
            if (serviceList.ContainsKey(host + ":" + port))
            {
                service = serviceList[host + ":" + port];
            }
            else { 
                service = new Service()
                {
                    Host = host,
                    Port = port,
                    ServiceId = 0,
                    ServiceName = "",
                    PollingFrequency = polling_frequency,//will set to min frequency with lowe limit of 1 second 
                    monitorServices = this
                };
                serviceList.Add(host + ":" + port,  service);
            }
            
            service.callersList.Add(caller);
            //service.ReportConnectionStatus += ReportedConnectionStatus;
            ReportNewRegister += RegisterNewCallerService;

            return true;
        }


        public void DoCheckConnection(CancellationToken Token)
        {
            while (!Token.IsCancellationRequested)
            {
                Parallel.ForEach(this.serviceList.Values.ToList(), (svc) =>
                {
                    svc.ReportedConnectionStatus += ReportedConnectionStatus;
                    svc.DoCheckConnection(Token);

                    Thread threadReg = new Thread(() => RegisterService(new object(), null));
                    threadReg.Start();
                    Thread threadOutage = new Thread(() => CreateServiceOutage(svc, null));
                    threadOutage.Start();

                });


            }

        }

        void CreateNewServiceOutage(object o, EventArgs e)
        {

        }
        void CreateServiceOutage(object o, EventArgs e)
        {
            var Inform = ServiceOutage;
            if (Inform != null)
            {
                Inform.Invoke(o, null);
                ServiceOutage = null;
            }

        }

        public void DoCreateServiceOutage(string host, string port,DateTime from,DateTime to)
        {
            ServiceOutage += CreateNewServiceOutage;
        }
    }
}
