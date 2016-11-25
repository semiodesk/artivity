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

using Artivity.DataModel;
using Artivity.Apid.Platforms;
using Artivity.Apid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semiodesk.Trinity;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Artivity.Apid.IO
{
    public class BackupWriter
    {
        #region Members

        private IModelProvider _modelProvider;

        private IPlatformProvider _platformProvider;

        #endregion

        #region Constructors

        public BackupWriter(IPlatformProvider platformProvider, IModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
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
                progressInfo.Total = 5;
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
                Logger.LogDebug("Compressing database..");

                writer.AddDirectory(baseFolder, _platformProvider.DatabaseFolder, new string[] { "*.lck" });

                if (progressInfo != null) progressInfo.Completed += 1;

                Logger.LogDebug("Compressing avatars..");

                writer.AddDirectory(baseFolder, _platformProvider.AvatarsFolder);

                if (progressInfo != null) progressInfo.Completed += 1;

                Logger.LogDebug("Compressing renderings..");

                writer.AddDirectory(baseFolder, _platformProvider.RenderingsFolder);

                if (progressInfo != null) progressInfo.Completed += 1;

                Logger.LogDebug("Compressing config file..");

                // Add the config file with optimal compression.
                writer.AddFile(baseFolder, _platformProvider.ConfigFile);

                if (progressInfo != null) progressInfo.Completed += 1;
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

        public long MaxBytesPerUpdate { get; set; }

        public long BytesWritten { get; private set; }

        #endregion

        #region Constructors

        public ZipFileWriter(string filePath)
        {
            FilePath = filePath;

            if (!File.Exists(filePath))
            {
                Archive = ZipFile.Open(filePath, ZipArchiveMode.Create);
            }
            else
            {
                Archive = ZipFile.Open(filePath, ZipArchiveMode.Update);
            }

            // Write max. 100 MB per update.
            MaxBytesPerUpdate = 100 * 1024 * 1024;
        }

        #endregion

        #region Methods

        private void WriteFile(string basePath, string filePath, CompressionLevel compressionLevel)
        {
            if(BytesWritten >= MaxBytesPerUpdate)
            {
                // Free the current archive handle memory.
                Archive.Dispose();

                // Re-open the ZIP archive handle for updating.
                Archive = ZipFile.Open(FilePath, ZipArchiveMode.Update);

                // Reset the byte count.
                BytesWritten = 0;
            }

            Logger.LogDebug("{0}", filePath);

            string entryName = filePath.Substring(basePath.Length + 1);

            // If the file cannot be read because it is opened by another process, try to read it manually.
            using (Stream entryStream = Archive.CreateEntry(entryName, compressionLevel).Open())
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.CopyTo(entryStream);

                    // Cannot access the properties of the archive entry while in create mode.
                    BytesWritten += fileStream.Length;
                }
            }
        }

        public void AddFile(string basePath, string filePath, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            if (!filePath.StartsWith(basePath))
            {
                throw new ArgumentException("The file path must be located inside the base directory.");
            }

            WriteFile(basePath, filePath, compressionLevel);
        }

        public void AddDirectory(string basePath, string directoryPath, string[] excludePatterns = null, string searchPattern = "*", CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            if (!directoryPath.StartsWith(basePath))
            {
                throw new ArgumentException("The folder path must be located inside the base directory.");
            }

            Logger.LogDebug("{0}", directoryPath);

            // Add the sub directories before updating the file with it's contents to preserve memory.
            foreach (string d in Directory.EnumerateDirectories(directoryPath))
            {
                AddDirectory(basePath, d, excludePatterns, searchPattern, compressionLevel);
            }

            // Update the existing archive to preserve memory.
            foreach (string f in Directory.EnumerateFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly))
            {
                if (excludePatterns != null && Regex.IsMatch(f, ToRegex(excludePatterns)))
                {
                    continue;
                }

                WriteFile(basePath, f, compressionLevel);
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
}
