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
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using MonoMac.CoreServices;
using MonoMac.Foundation;

namespace Artivity.Apid.Mac
{
    public class FileSystemWatcher
    {
        #region Members

        private TimeSpan _eventLatency = TimeSpan.FromMilliseconds(1000);

        private FSEventStream _eventStream;

        public string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    _path = value;

                    InitializeEventStream();
                }
            }
        }

        public string Filter { get; set; }

        //public System.IO.NotifyFilters NotifyFilter { get; set; }

        public bool IncludeSubdirectories { get; set; }

        private bool _enableRaisingEvents;

        public bool EnableRaisingEvents
        {
            get { return _enableRaisingEvents; }
            set
            {
                if (_enableRaisingEvents == value)
                {
                    return;
                }

                if(_eventStream != null)
                {
                    if(value)
                    {
                        _eventStream.Start();
                        _enableRaisingEvents = true;
                    }
                    else
                    {
                        _eventStream.Stop();
                        _enableRaisingEvents = false;
                    }
                }
            }
        }

        #endregion

        #region Constructors

        public FileSystemWatcher()
        {
        }

        #endregion

        #region Methods

        private void InitializeEventStream()
        {
            if (_eventStream != null)
            {
                _eventStream.Events -= OnEventStreamEvents;
                _eventStream.Dispose();
                _eventStream = null;
            }

            if (System.IO.Directory.Exists(_path))
            {
                _enableRaisingEvents = false;

                try
                {
                    Console.WriteLine("Path: {0} ; Latency: {1}", _path, _eventLatency);

                    _eventStream = new FSEventStream (new [] { _path }, _eventLatency, FSEventStreamCreateFlags.FileEvents);
                    _eventStream.Events += OnEventStreamEvents;
                    _eventStream.ScheduleWithRunLoop(NSRunLoop.Current);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("{0} : {1}", ex.Message, ex.InnerException);
                }
            }
        }

        private void OnEventStreamEvents(object sender, FSEventStreamEventsArgs e)
        {
            foreach (FSEvent ev in e.Events)
            {
                Console.WriteLine("{0}", ev.Path);
            }
        }

        #endregion

        #region Events

        //public event System.IO.FileSystemEventHandler Created;

        //public event System.IO.FileSystemEventHandler Deleted;

        //public event System.IO.FileSystemEventHandler Renamed;

        #endregion
    }
}

