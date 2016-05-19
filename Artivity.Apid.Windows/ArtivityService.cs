using Artivity.Api.Plugin;
using Artivity.WinService;
using log4net;
using log4net.Appender;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Artivity.Apid
{
    public class ArtivityService :  WindowsService
    {
        #region Members
        protected AutoResetEvent _stopping;
        protected ILog _log;
        protected EventLog eventLog;
        protected Dictionary<uint, HttpService> _services;

        public bool AutoPluginChecker { get; set; }

        private string _logConfigPath = "log.config";
        public string LogConfigPath { get { return _logConfigPath;} set{ _logConfigPath = value;} }
        #endregion

        #region Constructor

        public ArtivityService(string serviceName, string logConfig = null)
            : base(serviceName)
        {
            CanHandleSessionChangeEvent = true;
            AutoPluginChecker = true;

            if (!string.IsNullOrEmpty(logConfig))
                _logConfigPath = logConfig;
            InitializeLog();
            //_configuration = Configuration.Instance;

            //EventWaitHandleSecurity security = new System.Security.AccessControl.EventWaitHandleSecurity();
            //security.AddAccessRule(new EventWaitHandleAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), EventWaitHandleRights.Synchronize, AccessControlType.Allow));
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
            //_service = new HttpService();
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
        public override void InitialiseComponent()
        {
            CanStop = true;
            CanPauseAndContinue = false;
            CanHandleSessionChangeEvent = false;
            CanHandlePowerEvent = false;
            AutoLog = false;
            _stopping = new AutoResetEvent(false);
            _services = new Dictionary<uint, HttpService>();

            
        }


        void current_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.FatalFormat("Exception: {0}", e.ExceptionObject.ToString());
        }

        public void Start()
        {
            IInstallationWatchdog watchdog = null;
            if (AutoPluginChecker)
            {
                var check = PluginCheckerFactory.CreatePluginChecker();
                check.Check();
                watchdog = InstallationWatchdogFactory.CreateWatchdog();
                watchdog.ProgrammInstalledOrRemoved += (object s, EventArgs e) => { check.Check(); };
                watchdog.Start();
            }

            FindCurrentUsers();
            _log.DebugFormat("Waiting for user login...");
            _stopping.WaitOne();
            _log.Info("... Stopping");
            foreach (var session in _services)
                session.Value.Stop();

            if (AutoPluginChecker && watchdog != null)
            {
                watchdog.Stop();
            }
        }

        private void FindCurrentUsers()
        {
            var allUsers = Win32.GetCurrentUsers().Distinct().ToList();
            
            _log.DebugFormat("Users: {0}", string.Join(" ", allUsers));
            foreach (var user in allUsers)
            {
                if( !_services.ContainsKey(user.Item2))
                    StartService(user.Item2, user.Item1);
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

        protected void InitializeLog()
        {
            eventLog = new EventLog();
            eventLog.Log = "Application";
            eventLog.Source = base.ServiceName;
            ((ISupportInitialize)(this.EventLog)).BeginInit();
            if (!EventLog.SourceExists(this.EventLog.Source))
            {
                EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            }
            ((ISupportInitialize)(this.EventLog)).EndInit();

            FileInfo logFileConfig = new FileInfo(Path.Combine(GetCurrentDirectory(), LogConfigPath));
            log4net.Config.XmlConfigurator.Configure(logFileConfig);

            _log = LogManager.GetLogger("Artivity Service");

            AppDomain current = AppDomain.CurrentDomain;
            current.UnhandledException += current_UnhandledException;

            _log.Info("Service Initialized");
        }

        private string GetCurrentDirectory()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(this.GetType()).Location);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            /*_log.WriteEntry("Artivity.OnSessionChange " + DateTime.Now.ToLongTimeString() +
                " - Session change notice received: " +
                changeDescription.Reason.ToString() + "  Session ID: " +
                changeDescription.SessionId.ToString(), EventLogEntryType.Information);
            */
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    OnLogon((uint)changeDescription.SessionId);
                    break;

                case SessionChangeReason.SessionLogoff:
                    OnLogoff((uint)changeDescription.SessionId);
                    break;

            }
        }


        protected void OnLogon(uint sessionId)
        {
            string user = Win32.GetUsernameBySessionId((int)sessionId, true);
            _log.InfoFormat("Logon Event. SessionId {0} Username {1}", sessionId, user);
            StartService(sessionId, user);  
        }

        protected void StartService(uint sessionId, string user)
        {
            _log.DebugFormat("Starting service for user {0}", user);
            try
            {
                
                HttpService s = new HttpService();
               
                var action = new ParameterizedThreadStart(obj => 
                    {
                        //Thread.CurrentPrincipal = new GenericPrincipal(new WindowsIdentity(token), new string[]{});
                       
                        var sid = Win32.GetSidByUsername(user);
                        string regKeyFolders = string.Format(@"HKEY_USERS\{0}\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", sid);
                        string regValueAppData = @"AppData";
                        string path = Registry.GetValue(regKeyFolders, regValueAppData, null) as string;
                        s.ApplicationData = path;
                        // Only set username, disregard domain
                        s.Username = user.Split('\\')[1];
                        s.Start(false);
                    });
                Thread starter = new Thread(action);
                starter.Start();

                _services.Add(sessionId, s);
                _log.DebugFormat("Service running...");
            }
            catch (Exception e)
            {
                _log.Fatal("Starting HttpService failed!", e);
            }
        }

        void OnLogoff(uint sessionId)
        {
            string user = Win32.GetUsernameBySessionId((int)sessionId, true);
            _log.InfoFormat("Logoff Event. SessionId {0} Username {1}", sessionId, user);
            if (_services.ContainsKey(sessionId))
            {
                _log.InfoFormat("Stopping Service for {0}", user);
                var service = _services[sessionId];
                service.Stop(false);
                _services.Remove(sessionId);
            }
            
        }

        protected override void OnStop()
        {
            if( _stopping != null)
                _stopping.Set();
        }

        #endregion
    }

    
}
