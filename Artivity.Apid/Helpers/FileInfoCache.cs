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

namespace Artivity.Apid
{
    /// <summary>
    /// Keeps an in-memory copy of the values from a FileInfo object.
    /// </summary>
    public class FileInfoCache
    {
        #region Members

        public Uri Url { get; private set; }

        public bool Exists { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public DateTime CreationTime { get; private set; }

        public DateTime LastAccessTime { get; private set; }

        public DateTime LastWriteTime { get; private set; }

        public long Length { get; private set; }

        #endregion

        #region Constructors

        public FileInfoCache(string fullName)
        {
            FileInfo info = new FileInfo(fullName);

            Url = new Uri("file://" + Uri.EscapeUriString(fullName));
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

