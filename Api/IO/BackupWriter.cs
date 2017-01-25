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

using Artivity.Api.Helpers;
using Artivity.Api.Platform;
using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Artivity.Api.IO
{
    public class BackupWriter
    {
        #region Members

        private IPlatformProvider _platformProvider;

        #endregion

        #region Constructors

        public BackupWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
        {
            _platformProvider = platformProvider;
        }

        #endregion

        #region Methods

        public async Task WriteAsync(string targetPath, TaskProgressInfo progressInfo = null)
        {
            await Task.Run(() => Write(targetPath, progressInfo));
        }

        public void Write(string targetPath, TaskProgressInfo progressInfo = null)
        {
            FileInfo targetFile = new FileInfo(targetPath);

            if (!targetFile.Directory.Exists)
            {
                throw new DirectoryNotFoundException("The target directory does not exist.");
            }

            if(progressInfo != null)
            {
                progressInfo.Total = 1;
                progressInfo.Completed = 0;
            }

            if (targetFile.Exists)
            {
                File.Delete(targetPath);
            }

            // The data folder is compressed without copying any files..
            FileInfo backupFile = CompressArtivityDataFolder(targetPath, progressInfo);

            // Finally move the temporary export file to the final destination.
            File.Move(backupFile.FullName, targetFile.FullName);

            if (progressInfo != null) progressInfo.Completed += 1;
        }

        private FileInfo CompressArtivityDataFolder(string targetPath, TaskProgressInfo progressInfo)
        {
            string targetFile = Path.Combine(_platformProvider.ExportFolder, FileNameEncoder.Encode(_platformProvider.Config.Uid) + ".artb");

            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            // Write into the ZIP file through a memory-conserving updater class.
            using (ZipFileWriter writer = new ZipFileWriter(targetFile))
            {
                // The base path will be subtracted from the file path to make the path relative to the ZIP archive root.
                string baseFolder = _platformProvider.ArtivityDataFolder;

                // Add the users' data directories without any compression since most of the files are already compressed.
                writer.AddDirectory(baseFolder, _platformProvider.DatabaseFolder, new string[] { "*.lck" });
                writer.AddDirectory(baseFolder, _platformProvider.AvatarsFolder);
                writer.AddDirectory(baseFolder, _platformProvider.RenderingsFolder);
                writer.AddFile(baseFolder, _platformProvider.ConfigFile);

                if (progressInfo != null)
                {
                    progressInfo.Total += writer.Entries.Count();

                    writer.OnEntryWritten = () =>
                    {
                        progressInfo.Completed += 1;
                    };
                }

                writer.Write();
            }

            return new FileInfo(targetFile);
        }

        #endregion
    }

    internal class ZipFileWriter : IDisposable
    {
        #region Members

        public ZipArchive Archive { get; private set; }

        public string FilePath { get; private set; }

        private readonly List<ZipFileWriterEntry> _entries = new List<ZipFileWriterEntry>();

        public IEnumerable<ZipFileWriterEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        public Action OnEntryWritten = null;

        #endregion

        #region Constructors

        public ZipFileWriter(string filePath)
        {
            FilePath = filePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            Archive = ZipFile.Open(filePath, ZipArchiveMode.Create);
        }

        #endregion

        #region Methods

        public void Write()
        {
            foreach (ZipFileWriterEntry entry in _entries)
            {
                WriteEntry(entry);
            }

            _entries.Clear();
        }

        private void WriteEntry(ZipFileWriterEntry entry)
        {
            //Logger.LogDebug("{0}", entry.FilePath);

            // If the file cannot be read because it is opened by another process, try to read it manually.
            using (Stream entryStream = Archive.CreateEntry(entry.EntryName, entry.CompressionLevel).Open())
            {
                using (FileStream fileStream = new FileStream(entry.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.CopyTo(entryStream);
                    fileStream.Flush();
                }

                entryStream.Flush();
            }

            if (OnEntryWritten != null)
            {
                OnEntryWritten.Invoke();
            }
        }

        public void AddFile(string baseDirectory, string filePath, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            if (!filePath.StartsWith(baseDirectory))
            {
                throw new ArgumentException("The file path must be located inside the base directory.");
            }

            _entries.Add(new ZipFileWriterEntry(baseDirectory, filePath, compressionLevel));
        }

        public void AddDirectory(string baseDirectory, string directoryPath, string[] excludePatterns = null, string searchPattern = "*", CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            if (!directoryPath.StartsWith(baseDirectory))
            {
                throw new ArgumentException("The folder path must be located inside the base directory.");
            }

            //Logger.LogDebug("{0}", directoryPath);

            // Add the sub directories before updating the file with it's contents to preserve memory.
            foreach (string d in Directory.EnumerateDirectories(directoryPath))
            {
                AddDirectory(baseDirectory, d, excludePatterns, searchPattern, compressionLevel);
            }

            // Update the existing archive to preserve memory.
            foreach (string f in Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly))
            {
                if (excludePatterns != null && Regex.IsMatch(f, ToRegex(excludePatterns)))
                {
                    continue;
                }

                _entries.Add(new ZipFileWriterEntry(baseDirectory, f, compressionLevel));
            }
        }

        private static string ToRegex(string[] excludePatterns)
        {
            if (excludePatterns == null)
            {
                return "*";
            }

            StringBuilder resultBuilder = new StringBuilder();

            for (int i = 0; i < excludePatterns.Length; i++)
            {
                string pattern = excludePatterns[i];

                resultBuilder.Append('(');
                resultBuilder.Append(pattern.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
                resultBuilder.Append(')');

                if (i < excludePatterns.Length - 1)
                {
                    resultBuilder.Append("|");
                }
            }

            return resultBuilder.ToString();
        }

        public void Dispose()
        {
            Archive.Dispose();
        }

        #endregion
    }

    internal struct ZipFileWriterEntry
    {
        #region Members

        public readonly string EntryName;

        public readonly string FilePath;

        public readonly CompressionLevel CompressionLevel;

        #endregion

        #region Constructors

        public ZipFileWriterEntry(string baseDirectory, string filePath, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            EntryName = filePath.Substring(baseDirectory.Length + 1);
            FilePath = filePath;
            CompressionLevel = compressionLevel;
        }

        #endregion
    }
}
