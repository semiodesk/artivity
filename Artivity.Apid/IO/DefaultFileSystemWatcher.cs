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

using System;
using System.IO;

namespace Artivity.Apid.IO
{
    public class DefaultFileSystemWatcher : IFileSystemWatcher
    {
        #region Members

        private readonly FileSystemWatcher _watcher = new FileSystemWatcher() { NotifyFilter = NotifyFilters.FileName, IncludeSubdirectories = true };

        public string Path
        {
            get { return _watcher.Path; }
            set { _watcher.Path = value; }
        }

        public string Filter
        {
            get { return _watcher.Filter; }
            set { _watcher.Filter = value; }
        }

        public bool EnableRaisingEvents
        {
            get { return _watcher.EnableRaisingEvents; }
            set { _watcher.EnableRaisingEvents = value; }
        }

        #endregion

        #region Constructors

        public DefaultFileSystemWatcher()
        {
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            _watcher.Dispose();
        }

        #endregion

        #region Events

        event FileSystemEventHandler IFileSystemWatcher.Created
        {
            add { _watcher.Created += value; }
            remove {  _watcher.Created -= value; }
        }

        event FileSystemEventHandler IFileSystemWatcher.Deleted
        {
            add { _watcher.Deleted += value; }
            remove {  _watcher.Deleted -= value; }
        }

        event RenamedEventHandler IFileSystemWatcher.Renamed
        {
            add { _watcher.Renamed += value; }
            remove {  _watcher.Renamed -= value; }
        }

        #endregion
    }
}

