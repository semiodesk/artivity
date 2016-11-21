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

        public void Write(string targetPath)
        {
            FileInfo targetFile = new FileInfo(targetPath);

            if (!targetFile.Directory.Exists)
            {
                throw new DirectoryNotFoundException("The target directory does not exist.");
            }

            if(targetFile.Exists)
            {
                File.Delete(targetPath);
            }

            // The data folder is compressed without copying any files..
            FileInfo backupFile = CompressArtivityDataFolder(targetPath);

            // Finally move the temporary export file to the final destination.
            File.Move(backupFile.FullName, targetFile.FullName);
        }

        private FileInfo CompressArtivityDataFolder(string targetPath)
        {
            string dataFolder = _platformProvider.ArtivityDataFolder;

            string targetFile = Path.Combine(_platformProvider.ExportFolder, FileNameEncoder.Encode(_platformProvider.Config.Uid) + ".artb");

            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            using(MemoryStream stream = new MemoryStream())
            {
                ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create);
                archive.Dispose();
            }

            // Add the users' data directories without any compression since most of the files are already compressed.
            AddDirectory(targetFile, _platformProvider.ArtivityDataFolder, _platformProvider.DatabaseFolder, new string[] { "*.lck" });
            AddDirectory(targetFile, _platformProvider.ArtivityDataFolder, _platformProvider.AvatarsFolder);
            AddDirectory(targetFile, _platformProvider.ArtivityDataFolder, _platformProvider.RenderingsFolder);

            // Add the config file with optimal compression.
            AddFile(targetFile, _platformProvider.ArtivityDataFolder, _platformProvider.ConfigFile);

            return new FileInfo(targetFile);
        }

        private static ZipArchiveEntry CreateEntry(ZipArchive archive, string baseDirectory, string filePath, CompressionLevel level)
        {
            string key = filePath.Substring(baseDirectory.Length + 1);

            Console.WriteLine(key);

            return archive.CreateEntry(key, level);
        }

        public static void AddFile(string archivePath, string baseDirectory, string filePath, CompressionLevel level = CompressionLevel.Optimal)
        {
            if (!filePath.StartsWith(baseDirectory))
            {
                throw new ArgumentException("The file path must be located inside the base directory.");
            }

            // Update the existing archive to preserve memory.
            ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Update);

            // This entry will be reused in case of errors which are handled by AddFile (i.e. IOException or OutOfMemoryException).
            ZipArchiveEntry entry = CreateEntry(archive, baseDirectory, filePath, level);

            AddFile(ref archive, archivePath, entry, filePath);

            archive.Dispose();
        }

        public static void AddDirectory(string archivePath, string baseDirectory, string directoryPath, string[] excludePatterns = null, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, CompressionLevel level = CompressionLevel.NoCompression)
        {
            if (!directoryPath.StartsWith(baseDirectory))
            {
                throw new ArgumentException("The folder path must be located inside the base directory.");
            }

            // Update the existing archive to preserve memory.
            ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Update);

            foreach (string filePath in Directory.GetFiles(directoryPath, searchPattern, searchOption))
            {
                if (excludePatterns != null && Regex.IsMatch(filePath, ToRegex(excludePatterns)))
                {
                    continue;
                }

                // This entry will be reused in case of errors which are handled by AddFile (i.e. IOException or OutOfMemoryException).
                ZipArchiveEntry entry = CreateEntry(archive, baseDirectory, filePath, level);

                AddFile(ref archive, archivePath, entry, filePath);
            }

            archive.Dispose();
        }

        private static void AddFile(ref ZipArchive archive, string archivePath, ZipArchiveEntry entry, string filePath, int retryCount = 0)
        {
            try
            {
                // If the file cannot be read because it is opened by another process, try to read it manually.
                using (Stream entryStream = entry.Open())
                {
                    using (BinaryWriter writer = new BinaryWriter(entryStream))
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader reader = new StreamReader(fileStream, Encoding.Default))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch(OutOfMemoryException)
            {
                if (retryCount == 0)
                {
                    // If memory allocation failed. Close the existing file stream and try it again once more.
                    archive.Dispose();
                    archive = ZipFile.Open(archivePath, ZipArchiveMode.Update);

                    AddFile(ref archive, archivePath, entry, filePath, 1);
                }
                else
                {
                    throw;
                }
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

        #endregion
    }
}
