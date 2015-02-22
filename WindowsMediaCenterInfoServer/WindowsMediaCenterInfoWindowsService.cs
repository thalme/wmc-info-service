using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;

namespace WindowsMediaCenterInfoServer
{
    class WindowsMediaCenterInfoWindowsService : ServiceBase
    {
        public const string WINDOWS_SERVICE_NAME = "Windows Media Center Info Service";
  
        private ServiceHost serviceHost = null;

        public WindowsMediaCenterInfoWindowsService()
        {
            ServiceName = WINDOWS_SERVICE_NAME;
        }

        public static void Main()
        {
            ServiceBase.Run(new WindowsMediaCenterInfoWindowsService());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            serviceHost = new ServiceHost(typeof(RecordingsInfoService));
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    
    }

}
