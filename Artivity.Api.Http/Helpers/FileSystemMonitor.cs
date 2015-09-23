using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Semiodesk.Trinity;
using Artivity.Model;
using System.Collections.Generic;
using Artivity.Model.ObjectModel;

namespace Artivity.Api.Http
{
    public class FileSystemMonitor : IDisposable
    {
        #region Members

        private IModel _model;

        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        private HashSet<FileSystemObject> _deletedFiles = new HashSet<FileSystemObject>();

        private Dictionary<string, FileSystemObject> _monitoredFiles = new Dictionary<string, FileSystemObject>();

        private static Dictionary<Uri, Uri> _monitoredFileUris = new Dictionary<Uri, Uri>();

        #endregion

        #region Constructors

        public FileSystemMonitor()
        {
            _model = GetModel(Models.Activities);

            foreach (FileInfo file in GetMonitoredFiles())
            {
                // We store an in-memory copy of the file's metadata, just in case the file gets deleted.
                _monitoredFiles.Add(file.FullName, new FileSystemObject(file));
            }

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Created += OnFileSystemObjectCreated;
            watcher.Deleted += OnFileSystemObjectDeleted;
            watcher.Renamed += OnFileSystemObjectRenamed;
            watcher.Path = GetUserDirectory();
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.IncludeSubdirectories = true;

            _watchers.Add(watcher);
        }

        #endregion

        #region Methods

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

        private IModel GetModel(Uri uri)
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            if (store.ContainsModel(uri))
            {
                return store.GetModel(uri);
            }
            else
            {
                return store.CreateModel(uri);
            }
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

        private IEnumerable<FileInfo> GetMonitoredFiles()
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>

                SELECT DISTINCT ?f ?url WHERE { ?s a nfo:FileDataObject . ?s prov:specializationOf ?f . ?f nfo:fileUrl ?url . }";

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = _model.ExecuteQuery(query);

            foreach (BindingSet binding in result.GetBindings())
            {
                Uri uri = new Uri(binding["f"].ToString());
                Uri url = new Uri(binding["url"].ToString());

                if (!_monitoredFileUris.ContainsKey(url))
                {
                    _monitoredFileUris[uri] = url;
                }

                yield return new FileInfo(url.AbsolutePath);
            }
        }

        private void OnFileSystemObjectRenamed(object sender, RenamedEventArgs e)
        {
            if (_monitoredFiles.ContainsKey(e.FullPath))
            {
                _monitoredFiles[e.FullPath] = new FileSystemObject(e.FullPath);
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
                _monitoredFiles[e.FullPath] = new FileSystemObject(e.FullPath);
            }
            else if (_deletedFiles.Count > 0)
            {
                FileInfo created = new FileInfo(e.FullPath);

                foreach (FileSystemObject deleted in _deletedFiles)
                {
                    if (deleted.Length != created.Length)
                        continue;

                    if(deleted.CreationTime == created.CreationTime || deleted.Name == created.Name)
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

        private void UpdateFileDataObject(string oldPath, string newPath)
        {
            _monitoredFiles.Remove(oldPath);
            _monitoredFiles.Add(newPath, new FileSystemObject(newPath));

            ResourceQuery f = new ResourceQuery(nfo.FileDataObject);
            f.Where(nfo.fileUrl, "file://" + oldPath);

            ResourceQuery s = new ResourceQuery(nfo.FileDataObject);
            s.Where(prov.specializationOf, f);

            FileDataObject state = _model.GetResources<FileDataObject>(s).FirstOrDefault();

            if (state != null && state.GenericEntity is FileDataObject)
            {
                FileDataObject file = state.GenericEntity as FileDataObject;
                file.Url = "file://" + newPath;
                file.Commit();

                Logger.LogInfo("Moved {0} to {1}", oldPath, newPath);
            }
            else
            {
                Logger.LogError("Found no file data object for {0}", oldPath);
            }
        }

        #endregion
    }
}

