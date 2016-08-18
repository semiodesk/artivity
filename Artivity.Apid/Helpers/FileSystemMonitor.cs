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
using System.Diagnostics;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Artivity.Apid.Platforms;
using Nancy;
using System.Threading.Tasks;
using System.Threading;

namespace Artivity.Apid
{
    /// <summary>
    /// Monitors and handles changes to files which are registered in the database.
    /// </summary>
    /// <todo>
    /// - Mark files in the database as deleted.
    /// </todo>
    /// <remarks>
    /// - In order to track moved files, we look for a sequence of events:
    ///   1. Create new file (target, file size is 0 when event occurs)
    ///   2. Delete old file (source, file size is 0 when event occurs)
    /// </remarks>
    public class FileSystemMonitor : IDisposable
    {
        #region Members

        private IStore _store;

        private IModel _model;

        private IModelProvider _modelProvider;

        private IPlatformProvider _platformProvider;

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

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// The common application data folders are ignored in file system events.
        /// </summary>
        private string _appFolder;
        private string _appDataRoaming;
        private string _appDataLocal;

        /// <summary>
        /// Maps root directories of ready drives to file system watchers.
        /// </summary>
        /// <remarks>
        /// Only listens to create events for tracking moved files.
        /// </remarks>
        private readonly Dictionary<string, IFileSystemWatcher> _watchers = new Dictionary<string, IFileSystemWatcher>();

        /// <summary>
        /// A timer used for querying for new drives in a regluar interval.
        /// </summary>
        private readonly System.Timers.Timer _driveWatchersTimer = new System.Timers.Timer() { Interval = 1000 };

        /// <summary>
        /// Drive types which should be monitored by the drive watchers.
        /// </summary>
        private readonly HashSet<DriveType> _driveTypes = new HashSet<DriveType>()
        {
            DriveType.Network,
            DriveType.Removable
        };

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

        #endregion

        #region Constructors

        // Private because of singleton.
        private FileSystemMonitor() {}

        #endregion

        #region Methods

        public void Initialize(IModelProvider modelProvider, IPlatformProvider platformProvider)
        {
            if (!IsInitialized)
            {
                Logger.LogInfo("Starting file system monitor.");

                _store = StoreFactory.CreateStore(modelProvider.ConnectionString);

                _model = _store.GetModel(modelProvider.Activities);
                _modelProvider = modelProvider;
                _platformProvider = platformProvider;

                _appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _appDataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _appDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                InitializeFileWatchers();
                InitializeMonitoredFiles();

                IsInitialized = true;
            }
        }

        private void InitializeMonitoredFiles()
        {
            ISparqlQuery query = new SparqlQuery(@"
                SELECT DISTINCT
                    ?file ?fileName ?folder ?folderUrl
                WHERE
                {
                    ?entity nie:isStoredAs ?file .

                    ?file
                        rdfs:label ?fileName;
                        nfo:belongsToContainer ?folder.

                    ?folder nie:url ?folderUrl .
                }");

            int n = 0;

            foreach (BindingSet binding in _model.GetBindings(query))
            {
                string folderUrl = binding["folderUrl"].ToString();

                if (!Uri.IsWellFormedUriString(folderUrl, UriKind.Absolute))
                {
                    continue;
                }
                else if (folderUrl[folderUrl.Length - 1] != Path.DirectorySeparatorChar)
                {
                    folderUrl += Path.DirectorySeparatorChar;
                }

                string fileName = binding["fileName"].ToString();

                if (string.IsNullOrEmpty(fileName))
                {
                    continue;
                }

                Uri url = new Uri(folderUrl + fileName);

                // Do not register files twice..
                if (_monitoredFileUris.ContainsKey(url.LocalPath))
                {
                    continue;
                }

                FileInfoCache file = new FileInfoCache(url.LocalPath);

                // Check if the monitored file still exists..
                if (!File.Exists(url.LocalPath))
                {
                    Logger.LogDebug("Indexed file does not exist: {0}", url.LocalPath);

                    _deletedFiles.Add(file);

                    // In order to improve start up performance, we add the file to the list 
                    // of deleted files so that the deletion can be handled later..
                    file.DeletionTimeUtc = DateTime.UtcNow;

                    // NOTE: ProcessDeletedFiles() assumes that the file is registered in _monitoredFileUris.
                }

                Uri uri = new Uri(binding["file"].ToString());

                _monitoredFileUris[url.LocalPath] = uri;
                _monitoredFiles[url.LocalPath] = file;

                Logger.LogDebug("{0} : {1}", binding["folderUrl"].ToString(), file.FullName);

                n++;
            }

            Logger.LogInfo("Monitoring {0} file(s).", n);
        }

        /// <summary>
        /// Installs new drive watchers for all ready/accessible drives.
        /// </summary>
        private void InitializeFileWatchers()
        {
            if (_platformProvider.IsWindows)
            {
                // Add watchers for all fixed hard drive partitions such as C:
                _driveTypes.Add(DriveType.Fixed);
            }
            else
            {
                // Add a watcher for the user's home folder.
                InstallMonitoring(_platformProvider.UserFolder);
            }

            foreach (DriveInfo drive in DriveInfo.GetDrives().Where(drive => drive.IsReady))
            {
                if (!_driveTypes.Contains(drive.DriveType))
                {
                    continue;
                }

                string root = drive.RootDirectory.FullName;

                if (!_watchers.ContainsKey(root))
                {
                    InstallMonitoring(root);
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
                if (!_driveTypes.Contains(drive.DriveType))
                {
                    continue;
                }

                string root = drive.RootDirectory.FullName;

                if (drive.IsReady && !_watchers.ContainsKey(root))
                {
                    InstallMonitoring(drive.RootDirectory.FullName);
                }
                else if (!drive.IsReady && _watchers.ContainsKey(root))
                {
                    InstallMonitoring(drive.RootDirectory.FullName);
                }
            }

            // Periodically process the list of deleted files.
            if (_deletedFiles.Count > 0)
            {
                ProcessDeletedFiles();
            }
        }

        /// <summary>
        /// Enable raising events for all registered file system watchers.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;

            foreach (IFileSystemWatcher watcher in _watchers.Values)
            {
                TryEnable(watcher);
            }

            _driveWatchersTimer.Enabled = IsEnabled;

            Logger.LogInfo("Started file system monitor.");
        }

        /// <summary>
        /// Enable raising events for a give file system watcher.
        /// </summary>
        public bool TryEnable(IFileSystemWatcher watcher)
        {
            try
            {
                watcher.EnableRaisingEvents = IsEnabled;

                Logger.LogInfo("Enabled device monitoring: {0}", watcher.Path);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());

                return false;
            }
        }

        /// <summary>
        /// Disable raising events for all registered file system watchers.
        /// </summary>
        public void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;

                foreach (IFileSystemWatcher watcher in _watchers.Values)
                {
                    watcher.EnableRaisingEvents = IsEnabled;

                    Logger.LogInfo("Disabled device monitoring: {0}", watcher.Path);
                }

                _driveWatchersTimer.Enabled = IsEnabled;

                Logger.LogInfo("Stopped file system monitor.");
            }
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                GC.SuppressFinalize(this);

                Disable();

                _driveWatchersTimer.Elapsed -= UpdateDriveWatchers;
                _driveWatchersTimer.Dispose();

                foreach (IFileSystemWatcher watcher in _watchers.Values)
                {
                    watcher.Dispose();
                }
            }
        }

        /// <summary>
        /// Install the file system watcher for a given directory.
        /// </summary>
        /// <param name="root">The local path of the directory.</param>
        private void InstallMonitoring(string root)
        {
            // Mark the watcher as being created, so that subsequent invokes from the update method do
            // not try to install it again.
            _watchers[root] = null;

            // Register a path root watcher for file creation events.
            // This is required since sometimes moved files appear as being deleted and subsequently created.
            IFileSystemWatcher watcher = FileSystemWatcherFactory.Create();
            watcher.Created += OnFileSystemObjectCreated;
            watcher.Deleted += OnFileSystemObjectDeleted;
            watcher.Renamed += OnFileSystemObjectRenamed;
            watcher.Path = root;

            _watchers[watcher.Path] = watcher;

            Logger.LogInfo("Installed device monitoring: {0}", watcher.Path);

            if (IsEnabled)
            {
                TryEnable(watcher);
            }
        }

        /// <summary>
        /// Uninstall the file system watcher for a given directory.
        /// </summary>
        /// <param name="root">The local path of the directory.</param>
        private void UninstallMonitoring(string root)
        {
            if (_watchers.ContainsKey(root))
            {
                IFileSystemWatcher watcher = _watchers[root];

                if(watcher == null)
                {
                    // Trying to remove a non initialized watcher.
                    return;
                }

                if(watcher.EnableRaisingEvents)
                {
                    watcher.EnableRaisingEvents = false;

                    Logger.LogInfo("Disabled device monitoring: {0}", watcher.Path);
                }

                watcher.Created -= OnFileSystemObjectCreated;
                watcher.Dispose();

                _watchers.Remove(root);

                Logger.LogInfo("Uninstalled device monitoring: {0}", root);
            }
        }

        /// <summary>
        /// Indicates if the file event is rooted in a system or user application path and can therefore be ignored.
        /// </summary>
        /// <param name="path">An absolute file system path.</param>
        /// <returns><c>true</c> if the path is rooted in a user or system application folder, <c>false</c> otherwhise.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        private bool IsAppFileEvent(string path)
        {
            string p = Path.GetDirectoryName(path);

            return p.StartsWith(_appDataRoaming) || p.StartsWith(_appDataLocal) || p.StartsWith(_appFolder);
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
            try
            {
                // Ignore application data events.
                if (IsAppFileEvent(e.FullPath)) return;

                FileInfo file = new FileInfo(e.FullPath);

                // NOTE: Accessing the file attributes may cause exceptions; that's why it is the last condition.
                if (!file.Exists || file.CreationTime == DateTime.MinValue || file.Attributes.HasFlag(_createdFileMask))
                {
                    return;
                }

                // We use the URL for normalizing the local path strings.
                FileInfoCache createdFile = new FileInfoCache(file);
                createdFile.IsLocked = true;

                if(_createdFiles.Any(f => f.FullName == file.FullName))
                {
                    // The file is already being processed.
                    return;
                }

                // To detect file moves, remember the hashcode for subsequent delete events.
                _createdFiles.Enqueue(createdFile);

                // Handle the event asynchronously and do not wait for the result.
                HandleFileSystemObjectCreatedAsync(createdFile, e);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        private async Task HandleFileSystemObjectCreatedAsync(FileInfoCache file, FileSystemEventArgs e)
        {
            try
            {
                // Wait until there are no more open file handles. The file is still being written.
                while (file.HasOpenFileHandle())
                {
                    await Task.Delay(500);
                }

                // Look into the list of deleted files to see if the file was moved.
                // NOTE: The last file in the queue is the newest.
                FileInfoCache deletedFile = _deletedFiles.LastOrDefault(h => h.IndexCode == file.IndexCode);

                if (deletedFile != null)
                {
                    Logger.LogDebug("Created {0} ; IndexCode {1}", file.Url.LocalPath, file.IndexCode);

                    if (deletedFile.Url != file.Url)
                    {
                        // Lock the file to prevent it from finally being marked as deleted.
                        deletedFile.IsLocked = true;

                        // We assume that a recently deleted file with the same size 
                        // and creation time of a monitored one is being moved.
                        UpdateFileDataObject(deletedFile.Url, file.Url);
                    }
                }
                else if (_monitoredFiles.ContainsKey(file.Url.LocalPath))
                {
                    Logger.LogDebug("Created {0} ; IndexCode {1}", file.Url.LocalPath, file.IndexCode);

                    // A monitored file is being replaced, update the database.
                    UpdateFileDataObject(file.Url);
                }

                // Finally, free the lock on the file.
                file.IsLocked = false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Handles rename of files in the file system.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Rename event parameters.</param>
        private void OnFileSystemObjectRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                // Ignore application data events.
                if (IsAppFileEvent(e.FullPath)) return;

                FileInfoCache file = new FileInfoCache(e.FullPath);

                string filePath = file.Url.LocalPath;

                if (!file.Exists || file.CreationTimeUtc == DateTime.MinValue)
                {
                    // The new file does not exist. It's probably in a virtual file system.
                    Logger.LogDebug("Rename ignored: {0}", filePath);

                    return;
                }

                FileInfoCache oldFile = new FileInfoCache(e.OldFullPath);

                string oldFilePath = oldFile.Url.LocalPath;

                if (_monitoredFiles.ContainsKey(oldFilePath))
                {
                    Logger.LogDebug("Renamed {0} -> {1}", oldFilePath, filePath);

                    if (oldFile.HasOpenFileHandle())
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
                else if (_monitoredFiles.ContainsKey(filePath))
                {
                    Logger.LogDebug("Renamed {0} -> {1}", oldFilePath, filePath);

                    // If the new file is being monitored, update the database.
                    UpdateFileDataObject(file.Url);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
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
                // Ignore application data events.
                if (IsAppFileEvent(e.FullPath)) return;

                UriRef url = new UriRef(e.FullPath);

                if (_monitoredFiles.ContainsKey(url.LocalPath))
                {
                    FileInfoCache file = _monitoredFiles[url.LocalPath];

                    Logger.LogDebug("Deleted {0} ; IndexCode {1}", url.LocalPath, file.IndexCode);

                    bool handled = false;

                    if (_createdFiles.Any(f => f.Name == file.Name))
                    {
                        // NOTE: The last file in the queue is the newest.
                        FileInfoCache createdFile = _createdFiles.LastOrDefault(f => f.Name == file.Name);

                        if (createdFile != null)
                        {
                            FileInfo createdFileInfo = new FileInfo(createdFile.FullName);

                            if (createdFileInfo.Exists && createdFileInfo.FullName != file.FullName && createdFileInfo.Length == file.Length)
                            {
                                // We assume that a recently deleted file with the same size 
                                // and creation time of a monitored one is being moved.
                                UpdateFileDataObject(file.Url, createdFile.Url);

                                handled = true;
                            }
                        }
                    }

                    if(!handled)
                    {
                        // Mark the file as deleted.
                        file.DeletionTimeUtc = DateTime.UtcNow;

                        // Add the cached file stats to the list of deleted files so
                        // it can be processed in subsequent create events..
                        _deletedFiles.Add(file);
                    }
                }

                // Clean up the created files queue.
                while (_createdFiles.Count > 50)
                {
                    _createdFiles.Dequeue();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Process the list of deleted files and set the deletion date if a 
        /// there has been no subsequent creation event for some time.
        /// </summary>
        private void ProcessDeletedFiles()
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                // NOTE: Cannot remove items from hashset while iterating.
                List<FileInfoCache> processedFiles = new List<FileInfoCache>();

                foreach (FileInfoCache deletedFile in _deletedFiles)
                {
                    if(deletedFile.IsLocked)
                    {
                        // The file is currently processed by another thread. Do nothing.
                        continue;
                    }

                    if (!deletedFile.DeletionTimeUtc.HasValue)
                    {
                        Logger.LogError("Deleted file has no deletion time set: {0}", deletedFile.FullName);
                        Logger.LogDebug("{0}", new StackTrace());

                        processedFiles.Add(deletedFile);

                        continue;
                    }

                    try
                    {
                        TimeSpan threshold = TimeSpan.FromSeconds(10);

                        if ((now - deletedFile.DeletionTimeUtc) >= threshold)
                        {
                            //Uri fileUri = _monitoredFileUris[deletedFile.FullName];

                            //FileDataObject file = _model.GetResource<FileDataObject>(fileUri);
                            //file.DeletionTimeUtc = deletedFile.DeletionTimeUtc.Value;
                            //file.Commit();

                            //if (_monitoredFiles.ContainsKey(deletedFile.FullName))
                            //{
                            //    _monitoredFiles.Remove(deletedFile.FullName);
                            //    _monitoredFileUris.Remove(deletedFile.FullName);
                            //}

                            //Logger.LogDebug("Deleted: {0} ; <{1}>", deletedFile.FullName, file.Uri);

                            processedFiles.Add(deletedFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex);
                        Logger.LogDebug("{0}", new StackTrace());

                        // Do not try to process a faulty file again.
                        processedFiles.Add(deletedFile);
                    }
                }

                // Clean up the list of deleted files.
                foreach (FileInfoCache processedFile in processedFiles)
                {
                    _deletedFiles.Remove(processedFile);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Create a new file data object or update an existing object in the database at the given URL.
        /// </summary>
        /// <param name="url">URI of the information element / entity.</param>
        /// <param name="url">URL of the file.</param>
        public void AddFile(UriRef uri, Uri url)
        {
            if (_monitoredFileUris.ContainsKey(url.LocalPath))
            {
                // Do not create a new file data object for an already existing file..
                UpdateFileDataObject(url);

                return;
            }

            if(string.IsNullOrEmpty(url.LocalPath) || !File.Exists(url.LocalPath))
            {
                Logger.LogError("File does not exist: {0}", url);
                Logger.LogDebug("{0}", new StackTrace());

                return;
            }

            try
            {
                FileInfoCache fileInfo = new FileInfoCache(url.LocalPath);

                _monitoredFiles[fileInfo.Url.LocalPath] = fileInfo;
                _monitoredFileUris[fileInfo.Url.LocalPath] = uri;

                Logger.LogInfo("Enabled monitoring: {0}", fileInfo.Url.LocalPath);

                // Create the file and folder data objects.
                UriRef folderUrl = new UriRef(Path.GetDirectoryName(fileInfo.Url.LocalPath));

                DirectoryInfo folderInfo = new DirectoryInfo(folderUrl.LocalPath);

                Folder folderObject;

                ISparqlQuery query = new SparqlQuery(@"
                    SELECT
                        ?uri ?time
                    WHERE
                    {
                        ?uri a nfo:Folder .
                        ?uri nie:url @url .
                        ?uri nie:lastModified ?time .
                    }
                    ORDER BY DESC(?time) LIMIT 1
                ");

                query.Bind("@url", folderUrl.AbsoluteUri);

                BindingSet bindings = _model.GetBindings(query).FirstOrDefault();

                if(bindings != null)
                {
                    UriRef folderUri = new UriRef(bindings["uri"].ToString());

                    folderObject = _model.GetResource<Folder>(folderUri);

                    Logger.LogDebug("Updated folder: {0} ; {1}", folderInfo.FullName, folderObject.Uri);
                }
                else
                {
                    folderObject = _model.CreateResource<Folder>();

                    Logger.LogDebug("Created folder: {0} ; {1}", folderInfo.FullName, folderObject.Uri);
                }

                folderObject.Url = folderUrl;
                folderObject.CreationTimeUtc = folderInfo.CreationTimeUtc;
                folderObject.LastModificationTimeUtc = folderInfo.LastWriteTimeUtc;
                folderObject.Commit();

                FileDataObject fileObject = _model.CreateResource<FileDataObject>(uri);
                fileObject.Name = fileInfo.Name;
                fileObject.ByteSize = fileInfo.Length;
                fileObject.Folder = folderObject;
                fileObject.CreationTimeUtc = fileInfo.CreationTimeUtc;
                fileObject.LastModificationTimeUtc = fileInfo.LastWriteTimeUtc;
                fileObject.LastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
                fileObject.Commit();

                Logger.LogInfo("Created file: {0} ; {1}", fileInfo.FullName, fileObject.Uri);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Unregister a file from monitoring.
        /// </summary>
        public void RemoveFile(string path)
        {
            try
            {
                // Normalize the file path.
                path = new FileInfoCache(path).Url.LocalPath;

                if (_monitoredFiles.ContainsKey(path))
                {
                    _monitoredFiles.Remove(path);

                    Logger.LogInfo("Disabled monitoring: {0}", path);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Update the file data object in the database.
        /// </summary>
        /// <param name="oldUrl">URL of the file in the database.</param>
        private void UpdateFileDataObject(Uri url)
        {
            if (!_monitoredFileUris.ContainsKey(url.LocalPath))
            {
                Logger.LogError("Trying to update a non-monitored file: {0}", url.LocalPath);
                Logger.LogDebug("{0}", new StackTrace());

                return;
            }

            Uri uri = _monitoredFileUris[url.LocalPath];

            try
            {
                FileInfo fileInfo = new FileInfo(url.LocalPath);

                if (fileInfo.Exists)
                {
                    // Remove any files that might be in the deleted queue.
                    _deletedFiles.RemoveWhere(f => f.FullName == fileInfo.FullName);

                    _monitoredFiles[url.LocalPath] = new FileInfoCache(url.LocalPath);
                    _monitoredFileUris[url.LocalPath] = uri;

                    // Update the file data object.
                    FileDataObject fileObject = _model.GetResource<FileDataObject>(uri);
                    fileObject.ByteSize = fileInfo.Length;
                    fileObject.CreationTimeUtc = fileInfo.CreationTimeUtc;
                    fileObject.LastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
                    fileObject.LastModificationTimeUtc = fileInfo.LastWriteTimeUtc;

                    // Remove any deletion dates.
                    foreach (DateTime v in fileObject.ListValues(nfo.deletionDate))
                    {
                        fileObject.RemoveProperty(nfo.deletionDate, v);
                    }

                    fileObject.Commit();

                    Logger.LogDebug("Updated file: {0} ; <{1}>", fileInfo.FullName, fileObject.Uri);

                    DirectoryInfo folderInfo = new DirectoryInfo(Path.GetDirectoryName(url.LocalPath));

                    // Update the folder data object.
                    Folder folderObject = fileObject.Folder;
                    folderObject.CreationTimeUtc = folderInfo.CreationTimeUtc;
                    folderObject.LastModificationTimeUtc = folderInfo.LastWriteTimeUtc;
                    folderObject.Commit();

                    Logger.LogDebug("Updated folder: {0} ; <{1}>", folderInfo.FullName, folderObject.Uri);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(HttpStatusCode.InternalServerError, e);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

        /// <summary>
        /// Update the file data object in the database and the associated directory watchers.
        /// </summary>
        /// <param name="oldUrl">URL of the file in the database.</param>
        /// <param name="newUrl">New URL of the file.</param>
        private void UpdateFileDataObject(Uri oldUrl, Uri newUrl)
        {
            if (!_monitoredFileUris.ContainsKey(oldUrl.LocalPath))
            {
                Logger.LogError("Trying to update a non-monitored file: {0}", oldUrl.LocalPath);
                Logger.LogDebug("{0}", new StackTrace());

                return;
            }

            Uri uri = _monitoredFileUris[oldUrl.LocalPath];

            try
            {
                Logger.LogDebug("Moved {0} -> {1}", oldUrl.LocalPath, newUrl.LocalPath);

                FileInfo fileInfo = new FileInfo(newUrl.LocalPath);

                if (!fileInfo.Exists)
                {
                    Logger.LogError("Trying to update a non-existend target file: {0}", newUrl.LocalPath);
                    Logger.LogDebug("{0}", new StackTrace());

                    return;
                }

                // Remove any files that might be in the deleted queue.
                _deletedFiles.RemoveWhere(f => f.FullName == fileInfo.FullName);

                // Unregister the old file from monitoring.
                if (_monitoredFiles.ContainsKey(oldUrl.LocalPath))
                {
                    _monitoredFiles.Remove(oldUrl.LocalPath);
                    _monitoredFileUris.Remove(oldUrl.LocalPath);
                }

                // Register the new file for monitoring.
                _monitoredFiles[newUrl.LocalPath] = new FileInfoCache(newUrl.LocalPath);
                _monitoredFileUris[newUrl.LocalPath] = uri;

                // Check if the folder has changed.
                Folder folderObject = null;

                string oldFolderPath = Path.GetDirectoryName(oldUrl.LocalPath);
                string newFolderPath = Path.GetDirectoryName(newUrl.LocalPath);

                if (oldFolderPath != newFolderPath)
                {
                    // Update the folder metadata.
                    UriRef folderUrl = new UriRef(newFolderPath);

                    // Now, check if there is a data object for the target folder.
                    ISparqlQuery query = new SparqlQuery(@"
                            SELECT
                                ?folder
                            WHERE
                            {
                                ?folder
                                    a nfo:Folder ;
                                    nie:url @url .
                            }
                            LIMIT 1
                        ");

                    query.Bind("@url", folderUrl.AbsoluteUri);

                    BindingSet bindings = _model.GetBindings(query).FirstOrDefault();

                    if (bindings != null)
                    {
                        UriRef folderUri = new UriRef(bindings["folder"].ToString());

                        // Yes, reuse it and update the folder metadata.
                        folderObject = _model.GetResource<Folder>(folderUri);

                        Logger.LogDebug("Updated folder: {0} ; <{1}>", newFolderPath, folderObject.Uri);
                    }
                    else
                    {
                        // No, create a new data object for the folder..
                        folderObject = _model.CreateResource<Folder>();

                        Logger.LogDebug("Created folder: {0} ; <{1}>", newFolderPath, folderObject.Uri);
                    }

                    // Update the creation and modification times of the folder.
                    DirectoryInfo folderInfo = new DirectoryInfo(newFolderPath);

                    folderObject.Url = folderUrl;
                    folderObject.CreationTimeUtc = folderInfo.CreationTimeUtc;
                    folderObject.LastModificationTimeUtc = folderInfo.LastWriteTimeUtc;
                    folderObject.Commit();
                }

                // Get the file data object from the database and update the metadata.
                FileDataObject fileObject = _model.GetResource<FileDataObject>(uri);
                fileObject.Name = fileInfo.Name;
                fileObject.ByteSize = fileInfo.Length;
                fileObject.CreationTimeUtc = fileInfo.CreationTimeUtc;
                fileObject.LastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
                fileObject.LastModificationTimeUtc = fileInfo.LastWriteTimeUtc;

                if (folderObject != null)
                {
                    fileObject.Folder = folderObject;
                }

                // Remove any deletion dates.
                foreach (DateTime v in fileObject.ListValues(nfo.deletionDate))
                {
                    fileObject.RemoveProperty(nfo.deletionDate, v);
                }

                fileObject.Commit();

                Logger.LogDebug("Updated file: {0} ; <{1}>", newUrl.LocalPath, fileObject.Uri);
            }
            catch (Exception e)
            {
                Logger.LogError(HttpStatusCode.InternalServerError, e);
                Logger.LogDebug("{0}", new StackTrace());
            }
        }

//        private void MergeDuplicateFolders()
//        {
//            ISparqlQuery query = new SparqlQuery(@"
//                SELECT DISTINCT
//                    ?uri ?url ?file ?modified
//                WHERE
//                {
//                    ?uri
//                        a nfo:Folder ;
//                        nie:url ?url ;
//                        nie:lastModified ?modified .
//
//                    ?other
//                        a nfo:Folder ;
//                        nie:url ?url .
//
//                    ?file nfo:belongsToContainer ?other .
//                }
//                ORDER BY DESC(?modified)
//            ");

//            IEnumerable<BindingSet> bindings = _model.GetBindings(query);

//            string targetUrl = "";
//            UriRef targetUri = null;

//            foreach(BindingSet b in bindings)
//            {
//                string folderUrl = b["url"].ToString();
//                UriRef folderUri = new UriRef(b["uri"].ToString());

//                if (targetUrl != folderUrl || targetUri == null)
//                {
//                    targetUrl = folderUrl;
//                    targetUri = folderUri;

//                    continue;
//                }

//                SparqlUpdate update = new SparqlUpdate(@"
//                    DELETE
//                    {
//                        ?file nfo:belongsToContainer @folderUri .
//                    }
//                    INSERT
//                    {
//                        ?file nfo:belongsToContainer @targetUri .
//                    }
//                    WHERE
//                    {
//                        ?file nfo:belongsToContainer @folderUri .
//                    }
//                ");

//                update.Bind("@folderUri", folderUri);
//                update.Bind("@targetUri", targetUri);

//                _model.ExecuteUpdate(update);
//            }
//        }

//        private void ClearOrphanedFolders()
//        {
//            ISparqlQuery query = new SparqlQuery(@"
//                SELECT
//                    ?folder COUNT(?file) AS ?fileCount
//                WHERE
//                {
//                    ?file nfo:belongsToContainer ?folder
//                }
//                GROUP BY
//                    ?folder ?fileCount HAVING (?fileCount = 0)
//            ");

//            IEnumerable<BindingSet> bindings = _model.GetBindings(query);

//            foreach(BindingSet b in bindings)
//            {
//                UriRef folder = new UriRef(b["folder"].ToString());

//                _model.DeleteResource(folder);
//            }
//        }

        #endregion
    }
}

