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
using Semiodesk.Trinity;

namespace Artivity.Apid
{
    /// <summary>
    /// Keeps an in-memory copy of the values from a FileInfo object.
    /// </summary>
    internal class FileInfoCache
    {
        #region Members

        public FileAttributes Attributes { get; private set; }

        public Uri Url { get; private set; }

        public bool Exists { get; private set; }

        public string Extension { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public DateTime CreationTime { get; private set; }

        public DateTime LastAccessTime { get; private set; }

        public DateTime LastWriteTime { get; private set; }

        public long Length { get; private set; }

        /// <summary>
        /// A value which is used to determine if two files at different locations are equal.
        /// </summary>
        public readonly int IndexCode = -1;

        #endregion

        #region Constructors

        public FileInfoCache(string fullName) : this(new FileInfo(fullName))
        {
        }

        public FileInfoCache(FileInfo file)
        {
            Name = file.Name;
            FullName = file.FullName;
            Url = file.ToUriRef();
            Extension = file.Extension;
            Exists = file.Exists;

            if (Exists)
            {
                Attributes = file.Attributes;
                CreationTime = file.CreationTime;
                LastAccessTime = file.LastAccessTime;
                LastWriteTime = file.LastWriteTime;
                Length = file.Length;
                IndexCode = Name.GetHashCode() + CreationTime.GetHashCode() + Length.GetHashCode();
            }
        }

        #endregion

        #region Methods

        public bool IsLocked()
        {
            if(!File.Exists(FullName))
            {
                return false;
            }

            FileInfo file = new FileInfo(FullName);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // The file is unavailable because it is:
                // - still being written to
                // - or being processed by another thread
                // - or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Url.LocalPath.GetHashCode();
        }

        #endregion
    }
}

