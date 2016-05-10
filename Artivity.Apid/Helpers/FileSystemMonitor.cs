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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Nancy;

namespace Artivity.Apid
{
    public class FileSystemMonitor : IDisposable
    {
        #region Members

        private static FileSystemMonitor _instance;

        public static FileSystemMonitor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileSystemMonitor();
                }

                return _instance;
            }
        }

        public bool IsInitialized { get; private set; }

        private IModel _model;

        private readonly List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        private readonly HashSet<FileInfoCache> _deletedFiles = new HashSet<FileInfoCache>();

        private readonly Dictionary<Uri, FileInfoCache> _monitoredFiles = new Dictionary<Uri, FileInfoCache>();

        private readonly static Dictionary<Uri, Uri> _monitoredFileUris = new Dictionary<Uri, Uri>();

        private readonly static HashSet<string> _monitoredDirectories = new HashSet<string>();

        #endregion

        #region Constructors

        private FileSystemMonitor() {}

        #endregion

        #region Methods

        public void Initialize(IModelProvider provider)
        {
            Logger.LogInfo("Starting file system monitor.");

            if (IsInitialized)
            {
                return;
            }

            _model = provider.GetActivities();

            InitializeMonitoredFiles();

            IsInitialized = true;
        }

        private void InitializeMonitoredFiles()
        {
            // We order by start time so that we get the latest version of a file, 
            // just in case there exist previous (deleted) versions.
            SparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT ?uri ?url WHERE
                {
                    ?activity prov:used ?uri .
                    ?activity prov:startedAtTime ?startTime .
                    ?uri a nfo:FileDataObject .
                    ?uri nfo:fileUrl ?url . FILTER(!ISIRI(?url))
                }
                ORDER BY DESC(?startTime)");

            foreach (BindingSet binding in _model.GetBindings(query))
            {
                string u = binding["url"].ToString();

                if (!Uri.IsWellFormedUriString(u, UriKind.Absolute))
                {
                    continue;
                }

                Uri url = new Uri(u);

                if (_monitoredFileUris.ContainsKey(url) || !File.Exists(url.LocalPath))
                {
                    continue;
                }

                Uri uri = new Uri(binding["uri"].ToString());

                FileInfoCache fileInfo = new FileInfoCache(url.LocalPath);

                _monitoredFileUris[url] = uri;
                _monitoredFiles[url] = fileInfo;

                if (!_monitoredDirectories.Contains(fileInfo.Url.LocalPath))
                {
                    CreateDirectoryWatcher(fileInfo);
                }
            }
        }

        public void Start()
        {
            foreach (FileSystemWatcher watcher in _watchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                Logger.LogInfo("Enabled monitoring: {0}", file);

                watcher.EnableRaisingEvents = true;
            }

            Logger.LogInfo("Started file system monitor.");
        }

        public void Stop()
        {
            foreach (FileSystemWatcher watcher in _watchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                Logger.LogInfo("Disabled monitoring: {0}", file);

                watcher.EnableRaisingEvents = false;
            }

            Logger.LogInfo("Started file system monitor.");
        }

        public void Dispose()
        {
            Stop();
        }
            
        private void CreateDirectoryWatcher(FileInfoCache fileInfo)
        {
            string path = fileInfo.Url.LocalPath;

            if(!Directory.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            if(Directory.Exists(path) && !_monitoredDirectories.Contains(path))
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Created += OnFileSystemObjectCreated;
                watcher.Deleted += OnFileSystemObjectDeleted;
                watcher.Renamed += OnFileSystemObjectRenamed;
                watcher.Path = path;
                watcher.Filter = "*";
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.IncludeSubdirectories = true;

                _watchers.Add(watcher);

                _monitoredDirectories.Add(path);
            }
        }

        public static Uri TryGetFileUri(string fileUrl)
        {
            Uri url = new Uri(fileUrl);

            return _monitoredFileUris.ContainsKey(url) ? _monitoredFileUris[url] : null;
        }

        public static Uri GetFileUri(string fileUrl)
        {
            Uri url = new Uri(fileUrl);

            if (_monitoredFileUris.ContainsKey(url))
            {
                return _monitoredFileUris[url];
            }
            else
            {
                Uri uri = UriRef.GetGuid("http://semiodesk.com/id/{0}");

                _monitoredFileUris[url] = uri;

                return uri;
            }
        }

        private void OnFileSystemObjectRenamed(object sender, RenamedEventArgs e)
        {
            FileInfoCache file = new FileInfoCache(e.FullPath);
            FileInfoCache oldFile = new FileInfoCache(e.OldFullPath);

            if (_monitoredFiles.ContainsKey(file.Url))
            {
                _monitoredFiles[file.Url] = file;
            }

            if (_monitoredFiles.ContainsKey(oldFile.Url))
            {
                UpdateFileDataObject(oldFile.Url, file.Url);
            }

            Logger.LogInfo("Renamed {0} -> {1}", oldFile.Url.LocalPath, file.Url.LocalPath);
        }

        private void OnFileSystemObjectDeleted(object sender, FileSystemEventArgs e)
        {
            FileInfoCache file = new FileInfoCache(e.FullPath);

            if (_monitoredFiles.ContainsKey(file.Url))
            {
                _deletedFiles.Add(_monitoredFiles[file.Url]);
            }

            // TODO: Mark the file as deleted if no sub-sequent create occurs.
        }

        private void OnFileSystemObjectCreated(object sender, FileSystemEventArgs e)
        {
            FileInfoCache file = new FileInfoCache(e.FullPath);

            if (_monitoredFiles.ContainsKey(file.Url))
            {
                _monitoredFiles[file.Url] = CreateFileDataObject(file.Url);
            }
            else if (_deletedFiles.Count > 0)
            {
                foreach (FileInfoCache deleted in _deletedFiles)
                {
                    if (deleted.CreationTime == file.CreationTime || deleted.Name == file.Name || deleted.Length == file.Length)
                    {
                        // We assume that a recently deleted file with the same size 
                        // and creation time of a monitored one is being moved.
                        UpdateFileDataObject(deleted.Url, file.Url);

                        _deletedFiles.Remove(deleted);

                        break;
                    }
                }
            }
         }

        public void AddFile(string path)
        {
            FileInfoCache file = new FileInfoCache(path);

            _monitoredFiles[file.Url] = file;
        }

        public void RemoveFile(string path)
        {
            FileInfoCache file = new FileInfoCache(path);

            if (_monitoredFiles.ContainsKey(file.Url))
            {
                _monitoredFiles.Remove(file.Url);
            }
        }

        private FileInfoCache CreateFileDataObject(Uri url)
        {
            FileInfoCache file = new FileInfoCache(url.LocalPath);

            SparqlQuery query = new SparqlQuery(@"
                SELECT ?uri WHERE
                {
                    ?uri a nfo:FileDataObject .
                    ?uri nfo:fileUrl @fileUrl . 
                }
                LIMIT 1");

            query.Bind("@fileUrl", file.Url.AbsoluteUri);

            BindingSet bindings = _model.GetBindings(query).FirstOrDefault();

            if (bindings != null)
            {
                _monitoredFileUris[file.Url] = new Uri(bindings["uri"].ToString());

                Logger.LogInfo("Updating {0}", file.Url.LocalPath);
            }
            else
            {
                FileDataObject f = _model.CreateResource<FileDataObject>();
                f.CreationTime = file.CreationTime;
                f.LastAccessTime = file.LastAccessTime;
                f.LastModificationTime = file.LastWriteTime;
                f.Url = file.Url.AbsoluteUri;
                f.Commit();

                _monitoredFileUris[file.Url] = f.Uri;

                Logger.LogInfo("Created {0}", file.Url.LocalPath);
            }

            return file;
        }

        private void UpdateFileDataObject(Uri oldUrl, Uri newUrl)
        {
            if (_monitoredFileUris.ContainsKey(oldUrl))
            {
                try
                {
                    Uri uri = _monitoredFileUris[oldUrl];

                    FileDataObject file = _model.GetResource<FileDataObject>(uri);
                    file.Url = newUrl.AbsoluteUri;
                    file.Commit();

                    if(_monitoredFiles.ContainsKey(oldUrl))
                    {
                        _monitoredFiles.Remove(oldUrl);
                    }

                    _monitoredFiles[newUrl] = new FileInfoCache(newUrl.LocalPath);
                    _monitoredFileUris.Remove(oldUrl);
                    _monitoredFileUris[newUrl] = file.Uri;

                    Logger.LogInfo("Moved {0} -> {1}", oldUrl.LocalPath, newUrl.LocalPath);
                }
                catch(Exception e)
                {
                    Logger.LogError(HttpStatusCode.InternalServerError, e);
                }
            }
            else
            {
                CreateFileDataObject(newUrl);
            }
        }

        #endregion
    }
}

