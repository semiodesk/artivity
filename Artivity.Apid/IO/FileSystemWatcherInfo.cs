using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artivity.Apid.IO
{
    internal class FileSystemWatcherInfo
    {
        #region Members

        public IFileSystemWatcher FileWatcher { get; private set; }

        public int FileCount { get; set; }

        #endregion

        #region Constructors

        public FileSystemWatcherInfo(IFileSystemWatcher fileWatcher, int fileCount)
        {
            FileWatcher = fileWatcher;
            FileCount = fileCount;
        }

        #endregion
    }
}
