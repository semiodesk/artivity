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
using System.Timers;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Nancy;

namespace Artivity.Apid
{
    /// <summary>
    /// Monitors and handles changes to files which are registered in the database.
    /// </summary>
    /// <todo>
    /// - Mark files in the database as deleted.
    /// </todo>
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

        public bool IsEnabled { get; private set; }

        private IModel _model;

        /// <summary>
        /// Maps root directories of ready drives to file system watchers.
        /// </summary>
        /// <remarks>
        /// Only listens to create events for tracking moved files.
        /// </remarks>
        private readonly Dictionary<string, FileSystemWatcher> _driveWatchers = new Dictionary<string, FileSystemWatcher>();

        /// <summary>
        /// A timer used for querying for new drives in a regluar interval.
        /// </summary>
        private readonly Timer _driveWatchersTimer = new Timer() { Interval = 1000 };

        /// <summary>
        /// File system watchers used monitor directories which contain indexed files.
        /// </summary>
        /// <remarks>
        /// Only listens to delete and rename events. However, sometimes move events appear
        /// as a combination of delete and create events, not particularly in this order.
        /// </remarks>
        private readonly List<FileSystemWatcher> _fileWatchers = new List<FileSystemWatcher>();

        /// <summary>
        /// A log of previously created files; used for locating moved files.
        /// </summary>
        private readonly Queue<FileInfoCache> _createdFiles = new Queue<FileInfoCache>();

        /// <summary>
        /// Files with these attributes are ignored by the drive watchers' create event handler.
        /// </summary>
        private readonly FileAttributes _createdFileMask = FileAttributes.System 
            | FileAttributes.Temporary
            | FileAttributes.Hidden
            | FileAttributes.ReadOnly
            | FileAttributes.Directory
            | FileAttributes.Offline;

        /// <summary>
        /// A list of monitored files which have been deleted; used for locating moved files.
        /// </summary>
        private readonly HashSet<FileInfoCache> _deletedFiles = new HashSet<FileInfoCache>();

        /// <summary>
        /// Maps monitored file paths to their cached file information.
        /// </summary>
        private readonly Dictionary<string, FileInfoCache> _monitoredFiles = new Dictionary<string, FileInfoCache>();

        /// <summary>
        /// Maps monitored file paths to their URIs in the database.
        /// </summary>
        private readonly static Dictionary<string, Uri> _monitoredFileUris = new Dictionary<string, Uri>();

        /// <summary>
        /// Maps monitored directory paths to their file system watchers.
        /// </summary>
        private readonly static Dictionary<string, FileSystemWatcherInfo> _monitoredDirectories = new Dictionary<string, FileSystemWatcherInfo>();

        #endregion

        #region Constructors

        // Private because of singleton.
        private FileSystemMonitor() {}

        #endregion

        #region Methods

        public void Initialize(IModelProvider provider)
        {
            if (!IsInitialized)
            {
                Logger.LogInfo("Starting file system monitor.");

                _model = provider.ActivitiesModel;

                InitializeDriveWatchers();
                InitializeMonitoredFiles();

                IsInitialized = true;
            }
        }

        private void InitializeMonitoredFiles()
        {
            // We order by start time so that we get the latest version of a file, 
            // just in case there exist previous (deleted) versions.
            SparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT ?entity ?url WHERE
                {
                    ?entity a nfo:FileDataObject .
                    ?entity nfo:fileUrl ?url .
                    ?entity nfo:fileLastModified ?time .

                    FILTER(!ISIRI(?url))
                }
                ORDER BY DESC(?time)");

            foreach (BindingSet binding in _model.GetBindings(query))
            {
                string u = binding["url"].ToString();

                if (!Uri.IsWellFormedUriString(u, UriKind.Absolute))
                {
                    continue;
                }

                Uri url = new Uri(u);

                if (_monitoredFileUris.ContainsKey(url.LocalPath) || !File.Exists(url.LocalPath))
                {
                    continue;
                }

                Uri uri = new Uri(binding["entity"].ToString());

                FileInfoCache file = new FileInfoCache(url.LocalPath);

                _monitoredFileUris[url.LocalPath] = uri;
                _monitoredFiles[url.LocalPath] = file;

                InstallMonitoring(file.Url.LocalPath);
            }
        }

        /// <summary>
        /// Installs new drive watchers for all ready/accessible drives.
        /// </summary>
        private void InitializeDriveWatchers()
        {
            foreach(DriveInfo drive in DriveInfo.GetDrives().Where(drive => drive.IsReady))
            {
                string root = drive.RootDirectory.FullName;

                if(!_driveWatchers.ContainsKey(root))
                {
                    InstallMonitoring(drive);
                }
            }

            _driveWatchersTimer.Elapsed += UpdateDriveWatchers;
        }

        /// <summary>
        /// Installs or uninstalls new drive watchers in a regular interval.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Timer event arguments.</param>
        private void UpdateDriveWatchers(object sender, ElapsedEventArgs e)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                string root = drive.RootDirectory.FullName;

                if (drive.IsReady && !_driveWatchers.ContainsKey(root))
                {
                    InstallMonitoring(drive);
                }
                else if(!drive.IsReady && _driveWatchers.ContainsKey(root))
                {
                    UninstallMonitoring(drive);
                }
            }
        }

        /// <summary>
        /// Enable raising events for all registered file system watchers.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;

            foreach (FileSystemWatcher watcher in _driveWatchers.Values)
            {
                watcher.EnableRaisingEvents = IsEnabled;

                Logger.LogInfo("Enabled device monitoring: {0}", watcher.Path);
            }

            foreach (FileSystemWatcher watcher in _fileWatchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                watcher.EnableRaisingEvents = IsEnabled;

                Logger.LogInfo("Enabled monitoring: {0}", file);
            }

            _driveWatchersTimer.Enabled = IsEnabled;

            Logger.LogInfo("Started file system monitor.");
        }

        /// <summary>
        /// Disable raising events for all registered file system watchers.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;

            foreach (FileSystemWatcher watcher in _driveWatchers.Values)
            {
                watcher.EnableRaisingEvents = IsEnabled;

                Logger.LogInfo("Disabled device monitoring: {0}", watcher.Path);
            }

            foreach (FileSystemWatcher watcher in _fileWatchers)
            {
                string file = Path.Combine(watcher.Path, watcher.Filter);

                watcher.EnableRaisingEvents = IsEnabled;

                Logger.LogInfo("Disabled monitoring: {0}", file);
            }

            _driveWatchersTimer.Enabled = IsEnabled;

            Logger.LogInfo("Stopped file system monitor.");
        }

        public void Dispose()
        {
            Disable();

            foreach(FileSystemWatcher watcher in _driveWatchers.Values)
            {
                watcher.Dispose();
            }

            foreach (FileSystemWatcher watcher in _fileWatchers)
            {
                watcher.Dispose();
            }
        }

        /// <summary>
        /// Install a file system watcher for a given file, if necessary.
        /// </summary>
        /// <param name="filePath">Path to a file in the local file system.</param>
        private void InstallMonitoring(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!_monitoredDirectories.ContainsKey(directory))
            {
                // Register the watcher only if the directory is not yet monitored.
                if (Directory.Exists(directory) && !_monitoredDirectories.ContainsKey(directory))
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Deleted += OnFileSystemObjectDeleted;
                    watcher.Renamed += OnFileSystemObjectRenamed;
                    watcher.Path = directory;
                    watcher.Filter = "*";
                    watcher.NotifyFilter = NotifyFilters.FileName;
                    watcher.IncludeSubdirectories = false;

                    // Set the initial monitored files count to one.
                    _monitoredDirectories[directory] = new FileSystemWatcherInfo(watcher, 1);

                    // Register the watcher for enabling/disabling it at a later time.
                    _fileWatchers.Add(watcher);

                    Logger.LogInfo("Installed monitoring: {0}", directory);

                    if(IsEnabled)
                    {
                        watcher.EnableRaisingEvents = true;

                        Logger.LogInfo("Enabled monitoring: {0}", watcher.Path);
                    }
                }
            }
            else
            {
                _monitoredDirectories[directory].FileCount++;
            }
        }

        /// <summary>
        /// Install the file system watcher for a given drive.
        /// </summary>
        /// <param name="filePath">A drive info object.</param>
        private void InstallMonitoring(DriveInfo drive)
        {
            string root = drive.RootDirectory.FullName;

            // Mark the watcher as being created, so that subsequent invokes from the update method do
            // not try to install it again.
            _driveWatchers[root] = null;

            // Register a path root watcher for file creation events.
            // This is required since sometimes moved files appear as being deleted and subsequently created.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Created += OnFileSystemObjectCreated;
            watcher.Path = root;
            watcher.Filter = "*";
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.IncludeSubdirectories = true;

            _driveWatchers[watcher.Path] = watcher;

            Logger.LogInfo("Installed device monitoring: {0}", watcher.Path);

            if (IsEnabled)
            {
                watcher.EnableRaisingEvents = true;

                Logger.LogInfo("Enabled device monitoring: {0}", watcher.Path);
            }
        }

        /// <summary>
        /// Uninstall the file system watcher for a given file.
        /// </summary>
        /// <param name="filePath">Path to a file in the local file system.</param>
        private void UninstallMonitoring(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (_monitoredDirectories.ContainsKey(directory))
            {
                FileSystemWatcherInfo watcherInfo = _monitoredDirectories[directory];

                // Decrease the number of monitored files in the directory.
                watcherInfo.FileCount--;

                // If there are no monitored files left, dispose the watcher.
                if (watcherInfo.FileCount == 0)
                {
                    _monitoredDirectories.Remove(directory);

                    _fileWatchers.Remove(watcherInfo.FileWatcher);

                    watcherInfo.FileWatcher.Dispose();

                    Logger.LogInfo("Uninstalled monitoring: {0}", directory);
                }
            }
        }

        /// <summary>
        /// Uninstall the file system watcher for a given drive.
        /// </summary>
        /// <param name="filePath">A drive info object.</param>
        private void UninstallMonitoring(DriveInfo drive)
        {
            string root = drive.RootDirectory.FullName;

            if (_driveWatchers.ContainsKey(root))
            {
                FileSystemWatcher watcher = _driveWatchers[root];
                watcher.Created -= OnFileSystemObjectCreated;
                watcher.Dispose();

                _driveWatchers.Remove(root);

                Logger.LogInfo("Uninstalled device monitoring: {0}", watcher.Path);
            }
        }

        /// <summary>
        /// Handle file creation events.
        /// </summary>
        /// <remarks>
        /// The function may be called very often since it listens to changes in the root directory
        /// of the device. However, we need to listen at the root directory level since a monitored file
        /// may be moved to any location in the file system. Therefore, it is important to rule out any 
        /// non-interesting events as quickly as possible.
        /// </remarks>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">File system event arguments.</param>
        private void OnFileSystemObjectCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);

            if (!file.Exists || file.CreationTime == DateTime.MinValue)
            {
                return;
            }

            try
            {
                // Accessing the file attributes may cause exceptions.
                if(file.Attributes.HasFlag(_createdFileMask))
                {
                    return;
                }

                // We use the URL for normalizing the local path strings.
                FileInfoCache createdFile = new FileInfoCache(file);

                // To detect file moves, remember the hashcode for subsequent delete events.
                _createdFiles.Enqueue(createdFile);

                if (_monitoredFiles.ContainsKey(createdFile.Url.LocalPath))
                {
                    Logger.LogInfo("Created {0} ; IndexCode {1}", createdFile.Url.LocalPath, createdFile.IndexCode);

                    // A monitored file is being replaced, update the database.
                    UpdateFileDataObject(createdFile.Url);
                }
                else
                {
                    // Look into the list of deleted files to see if the file was moved.
                    FileInfoCache deletedFile = _deletedFiles.FirstOrDefault(h => h.IndexCode == createdFile.IndexCode);

                    if (deletedFile != null)
                    {
                        Logger.LogInfo("Created {0} ; IndexCode {1}", createdFile.Url.LocalPath, createdFile.IndexCode);

                        _deletedFiles.Remove(deletedFile);

                        // We assume that a recently deleted file with the same size 
                        // and creation time of a monitored one is being moved.
                        UpdateFileDataObject(deletedFile.Url, createdFile.Url);
                    }
                }

                // Clean up the queue.
                while (_createdFiles.Count > 10)
                {
                    _createdFiles.Dequeue();
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Handles rename of files in the file system.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Rename event parameters.</param>
        private void OnFileSystemObjectRenamed(object sender, RenamedEventArgs e)
        {
            FileInfoCache file = new FileInfoCache(e.FullPath);

            if (!file.Exists || file.CreationTime == DateTime.MinValue)
            {
                // The new file does not exist. It's probably in a virtual file system.
                Logger.LogInfo("Rename ignored: {0}", file.Url.LocalPath);

                return;
            }

            try
            {
                FileInfoCache oldFile = new FileInfoCache(e.OldFullPath);

                Logger.LogInfo("Renamed {0} -> {1}", oldFile.Url.LocalPath, file.Url.LocalPath);

                if (_monitoredFiles.ContainsKey(oldFile.Url.LocalPath))
                {
                    if (oldFile.IsLocked())
                    {
                        // The old file still exists and is being processed. Therefore ignore the rename and update the old file.
                        UpdateFileDataObject(oldFile.Url);
                    }
                    else
                    {
                        // If the file was being monitored, register it and update the database with the new name.
                        UpdateFileDataObject(oldFile.Url, file.Url);
                    }
                }
                else if (_monitoredFiles.ContainsKey(file.Url.LocalPath))
                {
                    // If the new file is being monitored, update the database.
                    UpdateFileDataObject(file.Url);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Handle file deletion events.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">File system event arguments.</param>
        private void OnFileSystemObjectDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                Uri url = new FileInfo(e.FullPath).ToUriRef();

                if (_monitoredFiles.ContainsKey(url.LocalPath))
                {
                    FileInfoCache deletedFile = _monitoredFiles[url.LocalPath];

                    Logger.LogInfo("Deleted {0} ; IndexCode {1}", url.LocalPath, deletedFile.IndexCode);

                    // We do not need to monitor the files' directory anymore.
                    UninstallMonitoring(deletedFile.Url.LocalPath);

                    FileInfoCache createdFile = _createdFiles.FirstOrDefault(f => f.IndexCode == deletedFile.IndexCode);

                    if (createdFile != null)
                    {
                        // We assume that a recently deleted file with the same size 
                        // and creation time of a monitored one is being moved.
                        UpdateFileDataObject(deletedFile.Url, createdFile.Url);
                    }
                    else
                    {
                        // Add the cached file stats to the list of deleted files so
                        // it can be processed in subsequent create events..
                        _deletedFiles.Add(deletedFile);
                    }
                }
                else
                {
                    Logger.LogInfo("Deleted {0}", url.LocalPath);
                }

                // Clean up the created files queue.
                while (_createdFiles.Count > 10)
                {
                    _createdFiles.Dequeue();
                }

                // TODO: Mark the file as deleted if no sub-sequent create occurs.
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public void AddFile(string path)
        {
            FileInfoCache file = new FileInfoCache(path);

            _monitoredFiles[file.Url.LocalPath] = file;
        }

        public void RemoveFile(string path)
        {
            // Normalize the file path.
            path = new FileInfoCache(path).Url.LocalPath;

            if (_monitoredFiles.ContainsKey(path))
            {
                _monitoredFiles.Remove(path);
            }
        }

        /// <summary>
        /// Create a new file data object or update an existing object in the database at the given URL.
        /// </summary>
        /// <param name="url">URL of the file.</param>
        /// <returns>A FileInfoCache object.</returns>
        private FileInfoCache CreateFileDataObject(Uri url)
        {
            FileInfoCache file = new FileInfoCache(url.LocalPath);

            ISparqlQuery query = new SparqlQuery(@"
                ASK WHERE
                {
                    ?entity a nfo:FileDataObject .
                    ?entity nfo:fileUrl @fileUrl . 
                }");

            query.Bind("@fileUrl", file.Url.AbsoluteUri);

            ISparqlQueryResult result = _model.ExecuteQuery(query);

            if (result.GetAnwser())
            {
                // Update the existring file data object at the given URL.
                UpdateFileDataObject(url);
            }
            else
            {
                // Create and register a new file data object.
                FileDataObject f = _model.CreateResource<FileDataObject>();
                f.CreationTime = file.CreationTime;
                f.LastAccessTime = file.LastAccessTime;
                f.LastModificationTime = file.LastWriteTime;
                f.Url = file.Url.AbsoluteUri;
                f.Commit();

                _monitoredFileUris[file.Url.LocalPath] = f.Uri;

                Logger.LogInfo("Created {0}", file.Url.LocalPath);
            }

            return file;
        }

        /// <summary>
        /// Update the file data object in the database.
        /// </summary>
        /// <param name="oldUrl">URL of the file in the database.</param>
        private void UpdateFileDataObject(Uri url)
        {
            if (_monitoredFileUris.ContainsKey(url.LocalPath))
            {
                Uri uri = _monitoredFileUris[url.LocalPath];

                try
                {
                    FileDataObject file = _model.GetResource<FileDataObject>(uri);
                    file.Url = url.AbsoluteUri;
                    file.LastModificationTime = DateTime.UtcNow;
                    file.Commit();

                    _monitoredFiles[url.LocalPath] = new FileInfoCache(url.LocalPath);
                    _monitoredFileUris[url.LocalPath] = file.Uri;

                    Logger.LogInfo("Updated {0}: {1}", file.Uri, url.LocalPath);
                }
                catch (Exception e)
                {
                    Logger.LogError(HttpStatusCode.InternalServerError, e);
                }
            }
            else
            {
                Logger.LogError("Trying to update a non-monitored file: {0}", url.LocalPath);
            }
        }

        /// <summary>
        /// Update the file data object in the database and the associated directory watchers.
        /// </summary>
        /// <param name="oldUrl">URL of the file in the database.</param>
        /// <param name="newUrl">New URL of the file.</param>
        private void UpdateFileDataObject(Uri oldUrl, Uri newUrl)
        {
            if (_monitoredFileUris.ContainsKey(oldUrl.LocalPath))
            {
                Uri uri = _monitoredFileUris[oldUrl.LocalPath];

                try
                {
                    // Get the file data object from the database and update the metadata.
                    FileDataObject file = _model.GetResource<FileDataObject>(uri);
                    file.Url = newUrl.AbsoluteUri;
                    file.LastModificationTime = DateTime.UtcNow;
                    file.Commit();

                    // Unregister the old file from monitoring.
                    if (_monitoredFiles.ContainsKey(oldUrl.LocalPath))
                    {
                        _monitoredFiles.Remove(oldUrl.LocalPath);
                        _monitoredFileUris.Remove(oldUrl.LocalPath);
                    }

                    // Register the new file for monitoring.
                    _monitoredFiles[newUrl.LocalPath] = new FileInfoCache(newUrl.LocalPath);
                    _monitoredFileUris[newUrl.LocalPath] = file.Uri;

                    // See if the file was moved to a new directory and update the watchers.
                    UninstallMonitoring(oldUrl.LocalPath);

                    // Enable watching for changes in the new file directory.
                    InstallMonitoring(newUrl.LocalPath);

                    Logger.LogInfo("Moved {0} -> {1} ; Updated {2}", oldUrl.LocalPath, newUrl.LocalPath, file.Uri);
                }
                catch (Exception e)
                {
                    Logger.LogError(HttpStatusCode.InternalServerError, e);
                }
            }
            else
            {
                Logger.LogError("Trying to update a non-monitored file: {0}", oldUrl.LocalPath);
            }
        }

        #endregion
    }
}

