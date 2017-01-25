// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Microsoft.Win32;
using RegistryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Plugin.Win
{
    class ProgramInstallationWatchdog : IProgramInstallationWatchdog
    {
        #region Members

        private string _registryKey;

        //private RegistryMonitor _monitor32;
        private RegistryMonitor _monitor64;

        //private RegistryKey _key32;
        private RegistryKey _key64;

        #endregion

        #region Constructor

        public ProgramInstallationWatchdog(string registryKey)
        {
            _registryKey = registryKey;
        }

        #endregion

        #region Methods

        public void Start()
        {
            //_key32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(InstalledPrograms.RegistryKeyString);

            //_monitor32 = new RegistryMonitor(_key32);
            //_monitor32.RegChangeNotifyFilter = RegChangeNotifyFilter.Key;
            //_monitor32.RegChanged += RegChanged;
            //_monitor32.Start();

            _key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(_registryKey);

            _monitor64 = new RegistryMonitor(_key64);
            _monitor64.RegChangeNotifyFilter = RegChangeNotifyFilter.Key;
            _monitor64.RegChanged += RaiseProgrammInstalledOrRemoved;
            _monitor64.Start();
        }

        public void Stop()
        {
            //if (_monitor32 != null)
            //{
            //    _monitor32.Stop();
            //}

            //if (_key32 != null)
            //{
            //    _key32.Dispose();
            //}

            if (_monitor64 != null)
            {
                _monitor64.Stop();
            }

            if (_key64 != null)
            {
                _key64.Dispose();
            }
        }

        public void Dispose()
        {
        }

        #endregion

        #region Events

        public event HandleProgramInstalledOrRemoved ProgrammInstalledOrRemoved;

        void RaiseProgrammInstalledOrRemoved(object sender, EventArgs e)
        {
            if (ProgrammInstalledOrRemoved != null)
            {
                ProgrammInstalledOrRemoved(this, null);
            }
        }

        #endregion
    }
}
