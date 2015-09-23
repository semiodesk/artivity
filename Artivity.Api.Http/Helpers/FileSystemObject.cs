using System;
using System.IO;

namespace Artivity.Api.Http
{
    public class FileSystemObject
    {
        #region Members

        public bool Exists { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public DateTime CreationTime { get; private set; }

        public DateTime LastAccessTime { get; private set; }

        public DateTime LastWriteTime { get; private set; }

        public long Length { get; private set; }

        #endregion

        #region Constructors

        public FileSystemObject(string fullName) : this(new FileInfo(fullName))
        {
        }

        public FileSystemObject(FileInfo info)
        {
            Name = info.Name;
            FullName = info.FullName;
            Exists = info.Exists;

            if (Exists)
            {
                CreationTime = info.CreationTime;
                LastAccessTime = info.LastAccessTime;
                LastWriteTime = info.LastWriteTime;
                Length = info.Length;
            }
        }

        #endregion
    }
}

