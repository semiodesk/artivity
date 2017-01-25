using Artivity.Apid.Platforms;
using Artivity.Apid.Plugin;
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

        private string _logConfigPath = "log.config";

        public string LogConfigPath { get { return _logConfigPath;} set{ _logConfigPath = value;} }

        public bool Debug { get; set; }

        #endregion

        #region Constructor

        public ArtivityService(string serviceName, string logConfig = null)
            : base(serviceName)
        {
            CanHandleSessionChangeEvent = true;

            if (!string.IsNullOrEmpty(logConfig))
                _logConfigPath = logConfig;
            InitializeLog();
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
            FindCurrentUsers();
            _log.DebugFormat("Waiting for user login...");
            _stopping.WaitOne();
            _log.Info("... Stopping");
            foreach (var session in _services)
                session.Value.Stop();
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
            FileInfo logFileConfig = new FileInfo(Path.Combine(GetCurrentDirectory(), LogConfigPath));
            log4net.Config.XmlConfigurator.Configure(logFileConfig);

            _log = LogManager.GetLogger("Artivity Service");

            AppDomain current = AppDomain.CurrentDomain;
            current.UnhandledException += current_UnhandledException;
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
                HttpService httpService = new HttpService();

#if DEBUG
                //httpService.UpdateOntologies = true;
#endif
               
                var action = new ParameterizedThreadStart(obj => 
                    {
                        //Thread.CurrentPrincipal = new GenericPrincipal(new WindowsIdentity(token), new string[]{});
                       
                        // Only set username, disregard domain
                        string userName = user.Split('\\')[1];

                        var sid = Win32.GetSidByUsername(user);
                        string regKeyFolders = string.Format(@"HKEY_USERS\{0}\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", sid);
                        string regValueAppData = @"AppData";
                        string appData = Registry.GetValue(regKeyFolders, regValueAppData, null) as string;

                        string regValuePersonal = @"Personal";
                        string personalFolder = Registry.GetValue(regKeyFolders, regValuePersonal, null) as string;
                        PlatformProvider p = new PlatformProvider(appData, personalFolder, userName);
                        p.SetDeploymentDir(this.GetCurrentDirectory());
                        httpService.ApplicationData = appData;
                        httpService.Username = userName;
                        httpService.PlatformProvider = p;
                        
                        httpService.Start(false);
                    });

                Thread starter = new Thread(action);
                starter.Start();

                _services.Add(sessionId, httpService);

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
