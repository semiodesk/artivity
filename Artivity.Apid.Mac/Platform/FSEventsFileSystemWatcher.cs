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
//  Moritz Eberl <moritz@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using CoreServices;
using CoreFoundation;

namespace Artivity.Apid.Mac
{
    public class FSEventsFileSystemWatcher : IFileSystemWatcher, IDisposable
    {
        #region Members

        [DllImport("/System/Library/Frameworks/CoreServices.framework/CoreServices")]
        private static extern void FSEventStreamSetDispatchQueue(IntPtr handle, IntPtr queue);

        private TimeSpan _eventLatency = TimeSpan.FromMilliseconds(100);

        private FSEventStream _eventStream;

        private FSEvent? _lastEvent;

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

        private string _filter;

        private Regex _filterExpression;

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;

                if (!string.IsNullOrEmpty(value))
                {
                    string expression = value.Replace(".", "[.]").Replace("*", ".*").Replace("?", ".");

                    _filterExpression = new Regex(expression);
                }
                else
                {
                    _filterExpression = null;
                }
            }
        }

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

        #region Methods

        public void Dispose()
        {
            if (_eventStream != null)
            {
                _eventStream.Stop();
                _eventStream.Events -= OnEventStreamEvents;
                _eventStream.Dispose();
                _eventStream = null;
            }
        }

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

                FSEventStreamCreateFlags flags = FSEventStreamCreateFlags.NoDefer | FSEventStreamCreateFlags.FileEvents;

                _eventStream = new FSEventStream (new [] { _path }, _eventLatency, flags);
                _eventStream.Events += OnEventStreamEvents;

                FSEventStreamSetDispatchQueue(_eventStream.Handle, DispatchQueue.MainQueue.Handle);

            }
        }

        private void OnEventStreamEvents(object sender, FSEventStreamEventsArgs args)
        {
            for(int i = 0; i < args.Events.Length; i++)
            {
                FSEvent e = args.Events[i];

                if (e.Flags.HasFlag(FSEventStreamEventFlags.ItemCreated))
                {                   
                    // For some reason, we receive created events multiple times in a sequence.
                    if (!_lastEvent.HasValue || _lastEvent.HasValue && _lastEvent.Value.Flags != e.Flags && _lastEvent.Value.Path != e.Path)
                    {
                        Raise(Created, System.IO.WatcherChangeTypes.Created, e.Path);
                    }
                }
                else if (e.Flags.HasFlag(FSEventStreamEventFlags.ItemRemoved))
                {
                    Raise(Deleted, System.IO.WatcherChangeTypes.Deleted, e.Path);
                }
                else if (e.Flags.HasFlag(FSEventStreamEventFlags.ItemRenamed) && i < args.Events.Length - 1)
                {
                    RaiseRenamed(args.Events[i + 1].Path, args.Events[i].Path);

                    // Skip the next event.
                    i++;
                }
            }

            // We store the last event for filtering out duplicate Create-events.
            _lastEvent = args.Events[args.Events.Length - 1];
        }

        private void Raise(System.IO.FileSystemEventHandler handler, System.IO.WatcherChangeTypes changeType, string path)
        {
            if (handler!= null)
            {
                if (_filterExpression != null && !_filterExpression.IsMatch(path))
                {
                    return;
                }

                string name = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path);

                handler(this, new System.IO.FileSystemEventArgs(changeType, directory, name));
            }
        }

        private void RaiseRenamed(string path, string oldPath)
        {
            if(Renamed != null)
            {
                if (_filterExpression != null && !_filterExpression.IsMatch(path) && !_filterExpression.IsMatch(oldPath))
                {
                    return;
                }

                string directory = System.IO.Path.GetDirectoryName(path);
                string oldDirectory = System.IO.Path.GetDirectoryName(oldPath);

                // This resembles the behavior of the default .NET FileSystemWatcher.
                if(oldDirectory == directory)
                {
                    string name = System.IO.Path.GetFileName(path);
                    string oldName = System.IO.Path.GetFileName(oldPath);

                    Renamed(this, new System.IO.RenamedEventArgs(System.IO.WatcherChangeTypes.Renamed, directory, name, oldName));
                }
                else
                {
                    Raise(Deleted, System.IO.WatcherChangeTypes.Deleted, oldPath);
                    Raise(Created, System.IO.WatcherChangeTypes.Created, path);
                }
            }
        }

        #endregion

        #region Events

        public event System.IO.FileSystemEventHandler Created;

        event System.IO.FileSystemEventHandler IFileSystemWatcher.Created
        {
            add { Created += value; }
            remove { Created -= value; }
        }

        public event System.IO.FileSystemEventHandler Deleted;

        event System.IO.FileSystemEventHandler IFileSystemWatcher.Deleted
        {
            add { Deleted += value; }
            remove { Deleted -= value; }
        }

        public event System.IO.RenamedEventHandler Renamed;

        event System.IO.RenamedEventHandler IFileSystemWatcher.Renamed
        {
            add { Renamed += value; }
            remove { Renamed -= value; }
        }

        #endregion
    }
}

