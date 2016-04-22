using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace Artivity.Apid
{

    public abstract class WindowsService : ServiceBase
    {
        WindowsServiceInstaller Installer = null;

        public WindowsService(string serviceName)
        {
            ServiceName = serviceName;
            InitialiseComponent();
        }

        public void Install()
        {
            if (Installer == null)
            {
                throw new Exception("Error: You need to either create an installer or supply your own.");
            }

            Install(Installer);
        }

        public void Install(Installer installer)
        {
            TransactedInstaller ti = new TransactedInstaller();
            ti.Installers.Add(installer);
            String path = String.Format("/assemblypath={0}",
                                        Assembly.GetExecutingAssembly().Location);
            String[] cmdline = { path };
            InstallContext ctx = new InstallContext("", cmdline);
            ti.Context = ctx;
            ti.Install(new Hashtable());
        }

        public void Uninstall()
        {
            if (Installer == null)
            {
                throw new Exception("Error: You need to either create an installer or supply your own.");
            }
            Uninstall(Installer);
        }

        public void Uninstall(Installer installer)
        {
            TransactedInstaller ti = new TransactedInstaller();
            ti.Installers.Add(installer);
            String path = String.Format("/assemblypath={0}",
                                        Assembly.GetExecutingAssembly().Location);
            String[] cmdline = { path };
            InstallContext ctx = new InstallContext("", cmdline);
            ti.Context = ctx;
            ti.Uninstall(null);
        }


        public void Run()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { this };
            ServiceBase.Run(ServicesToRun);
        }

        /// <summary>
        /// Set the following properties:
        /// CanStop = true;
        /// CanPauseAndContinue = false;
        /// CanHandleSessionChangeEvent = false;
        /// CanHandlePowerEvent = false;
        /// AutoLog = true;
        /// </summary>
        public virtual void InitialiseComponent()
        {

        }

        public void CreateInstaller(string displayName, ServiceAccount account, ServiceStartMode startMode)
        {
            Installer = new WindowsServiceInstaller(ServiceName, displayName, account, startMode);
        }

        [RunInstaller(true)]
        public class WindowsServiceInstaller : Installer
        {

            public WindowsServiceInstaller(string serviceName, string displayName, ServiceAccount account, ServiceStartMode startMode)
            {
                ServiceProcessInstaller spi = new ServiceProcessInstaller();

                ServiceInstaller si = new ServiceInstaller();
                spi.Account = account;
                si.ServiceName = serviceName;
                si.DisplayName = displayName;
                si.StartType = startMode;

                this.Installers.Add(spi);
                this.Installers.Add(si);

            }
        }
    }

}
