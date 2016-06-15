
using Microsoft.Win32;
using RegistryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Artivity.Api.Plugin.OSX
{

    class InstallationWatchdog : IInstallationWatchdog
    {
        

        #region Members
        public event HandleProgramInstalledOrRemoved ProgrammInstalledOrRemoved;

        Timer _timer;

        public TimeSpan TimerInterval 
        { 
            get 
            {
                return TimeSpan.FromMilliseconds (_timer.Interval);
            }
            set
            {
                _timer.Interval = value.TotalMilliseconds;
            }
        }
        #endregion

        #region Constructor
        public InstallationWatchdog()
        {
            _timer = new Timer ();  
            _timer.AutoReset = true;
            TimerInterval = TimeSpan.FromMinutes (1);
        }
        #endregion

        #region Methods
        public void Start()
        {
            _timer.Elapsed += TimerElapsed;
            _timer.Start ();
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timer.Enabled) 
            {
                if( ProgrammInstalledOrRemoved != null )
                    ProgrammInstalledOrRemoved (this, null);
            }
        }

        public void Stop()
        {
            _timer.Stop ();
            _timer.Elapsed -= TimerElapsed;
        }

        public void Dispose ()
        {
            _timer.Dispose ();
        }
        #endregion
    }
}
