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

namespace Artivity.Api.Http
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

        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        private HashSet<FileInfoCache> _deletedFiles = new HashSet<FileInfoCache>();

        private Dictionary<string, FileInfoCache> _monitoredFiles = new Dictionary<string, FileInfoCache>();

        private static Dictionary<Uri, Uri> _monitoredFileUris = new Dictionary<Uri, Uri>();

        #endregion

        #region Constructors

        private FileSystemMonitor() {}

        #endregion

        #region Methods

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }
            
            _model = Models.GetActivities();

            InitializeMonitoredFiles();

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Created += OnFileSystemObjectCreated;
            watcher.Deleted += OnFileSystemObjectDeleted;
            watcher.Renamed += OnFileSystemObjectRenamed;
            watcher.Path = GetUserDirectory();
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.IncludeSubdirectories = true;

            _watchers.Add(watcher);

            IsInitialized = true;
        }

        private void InitializeMonitoredFiles()
        {
            // We order by start time so that we get the latest version of a file, 
            // just in case there exist previous (deleted) versions.
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT DISTINCT ?uri ?url WHERE
                {
                    ?activity prov:used ?uri .
                    ?activity prov:startedAtTime ?startTime .
                    ?uri a nfo:FileDataObject .
                    ?uri nfo:fileUrl ?url . FILTER(!ISIRI(?url))
                }
                ORDER BY DESC(?startTime)";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = _model.ExecuteQuery(query);

            foreach (BindingSet binding in result.GetBindings())
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

                _monitoredFileUris[url] = uri;
                _monitoredFiles[url.LocalPath] = new FileInfoCache(url.LocalPath);
            }
        }

        public void Start()
        {
            foreach (FileSystemWatcher watcher in _watchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                Logger.LogInfo("Started monitoring {0}", file);

                watcher.EnableRaisingEvents = true;
            }
        }

        public void Stop()
        {
            foreach (FileSystemWatcher watcher in _watchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                Logger.LogInfo("Stopped monitoring {0}", file);

                watcher.EnableRaisingEvents = false;
            }
        }

        public void Dispose()
        {
            Stop();
        }
            
        private static string GetUserDirectory()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return Environment.GetEnvironmentVariable("HOME");
            }
            else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            }
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else
            {
                throw new PlatformNotSupportedException();
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
            if (_monitoredFiles.ContainsKey(e.FullPath))
            {
                Logger.LogInfo("Renamed {0}", e.FullPath);

                _monitoredFiles[e.FullPath] = new FileInfoCache(e.FullPath);
            }
            else if (_monitoredFiles.ContainsKey(e.OldFullPath))
            {
                UpdateFileDataObject(e.OldFullPath, e.FullPath);
            }
        }

        private void OnFileSystemObjectDeleted(object sender, FileSystemEventArgs e)
        {
            if (_monitoredFiles.ContainsKey(e.FullPath))
            {
                _deletedFiles.Add(_monitoredFiles[e.FullPath]);
            }

            // TODO: Mark the file as deleted if no sub-sequent create occurs.
        }

        private void OnFileSystemObjectCreated(object sender, FileSystemEventArgs e)
        {
            if (_monitoredFiles.ContainsKey(e.FullPath))
            {
                _monitoredFiles[e.FullPath] = CreateFileDataObject(e.FullPath);
            }
            else if (_deletedFiles.Count > 0)
            {
                FileInfoCache created = new FileInfoCache(e.FullPath);

                foreach (FileInfoCache deleted in _deletedFiles)
                {
                    if(deleted.CreationTime == created.CreationTime || deleted.Name == created.Name || deleted.Length == created.Length)
                    {
                        // We assume that a recently deleted file with the same size 
                        // and creation time of a monitored one is being moved.
                        UpdateFileDataObject(deleted.FullName, created.FullName);

                        _deletedFiles.Remove(deleted);

                        break;
                    }
                }
            }
         }

        public void AddFile(string path)
        {
            _monitoredFiles[path] = new FileInfoCache(path);
        }

        public void RemoveFile(string path)
        {
            if (_monitoredFiles.ContainsKey(path))
            {
                _monitoredFiles.Remove(path);
            }
        }

        private FileInfoCache CreateFileDataObject(string path)
        {
            FileInfoCache file = new FileInfoCache(path);

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT ?uri WHERE
                {
                    ?uri a nfo:FileDataObject .
                    ?uri nfo:fileUrl """ + file.Url.AbsoluteUri + @""" . 
                }
                LIMIT 1";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = _model.ExecuteQuery(query);

            BindingSet bindings = result.GetBindings().FirstOrDefault();

            if (bindings == null)
            {
                FileDataObject f = _model.CreateResource<FileDataObject>();
                f.CreationTime = file.CreationTime;
                f.LastAccessTime = file.LastAccessTime;
                f.LastModificationTime = file.LastWriteTime;
                f.Url = file.Url.AbsoluteUri;
                f.Commit();

                _monitoredFileUris[file.Url] = f.Uri;

                Logger.LogInfo("Created {0}", file.FullName);
            }
            else
            {
                _monitoredFileUris[file.Url] = new Uri(bindings["uri"].ToString());

                Logger.LogInfo("Updating {0}", file.FullName);
            }

            return file;
        }

        private void UpdateFileDataObject(string oldPath, string newPath)
        {
            Uri oldUrl = new Uri("file://" + Uri.EscapeUriString(oldPath));
            Uri newUrl = new Uri("file://" + Uri.EscapeUriString(newPath));

            if (_monitoredFileUris.ContainsKey(oldUrl))
            {
                try
                {
                    Uri uri = _monitoredFileUris[oldUrl];

                    FileDataObject file = _model.GetResource<FileDataObject>(uri);
                    file.Url = newUrl.AbsoluteUri;
                    file.Commit();

                    _monitoredFiles.Remove(oldPath);
                    _monitoredFiles.Add(newPath, new FileInfoCache(newPath));
                    _monitoredFileUris.Remove(oldUrl);
                    _monitoredFileUris.Add(newUrl, file.Uri);

                    Logger.LogInfo("Moved {0}", newPath);
                }
                catch(Exception e)
                {
                    Logger.LogError(HttpStatusCode.InternalServerError, e);
                }
            }
            else
            {
                CreateFileDataObject(newPath);
            }
        }

        #endregion
    }
}

