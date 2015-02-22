using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsMediaCenterInfoServer
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {

        public ProjectInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            Installers.Add(process);

            ServiceInstaller service = new ServiceInstaller();
            service.ServiceName = WindowsMediaCenterInfoWindowsService.WINDOWS_SERVICE_NAME;
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(service);
        }

    }
}
