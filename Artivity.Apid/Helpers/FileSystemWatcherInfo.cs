using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artivity.Apid
{
    internal class FileSystemWatcherInfo
    {
        #region Members

        public FileSystemWatcher FileWatcher { get; private set; }

        public int FileCount { get; set; }

        #endregion

        #region Constructors

        public FileSystemWatcherInfo(FileSystemWatcher fileWatcher, int fileCount)
        {
            FileWatcher = fileWatcher;
            FileCount = fileCount;
        }

        #endregion
    }
}
