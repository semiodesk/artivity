using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Artivity.Api.Http
{
    public class ArtivityService :  WindowsService
    {
        #region Members
        protected HttpService _service;
        #endregion

        #region Constructor

        public ArtivityService(string serviceName)
            : base(serviceName)
        {
            //_configuration = Configuration.Instance;

            EventWaitHandleSecurity security = new System.Security.AccessControl.EventWaitHandleSecurity();
            security.AddAccessRule(new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.Synchronize, AccessControlType.Allow));
            //bool created = false;
            //#if DEBUG
            //string mode = "DEBUG";
            //#else
            //string mode = "RELEASE";
            //#endif
            //string nameAdditon = "";
            //if( IntPtr.Size == 4 )
            //    nameAdditon = "32";
            //else
            //    nameAdditon = "64";
            //StopEvent = new EventWaitHandle(false, EventResetMode.ManualReset, string.Format("Global\\OrganiseMetaSync_StopEvent_{0}_{1}", mode, nameAdditon), out created, security);
            _service = new HttpService();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Install the service in the system.
        /// </summary>
        public new void Install()
        {
            base.Install();

        }

        /// <summary>
        /// Removes the service from the system.
        /// </summary>
        public new void Uninstall()
        {
            base.Uninstall();

        }

        /// <summary>
        /// Needed to set the initial variables.
        /// </summary>
        private void InitializeComponent()
        {
            CanStop = true;
            CanPauseAndContinue = false;
            CanHandleSessionChangeEvent = false;
            CanHandlePowerEvent = false;
            AutoLog = true;
        }

        public void Start()
        {
            try
            {
                _service.Start(false);
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "Artivity Service";
                appLog.WriteEntry(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Thread startThread = new Thread(Start);
            startThread.Start();
        }

        

        protected override void OnStop()
        {
            _service.Stop();
        }

        #endregion
    }

    
}
