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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Semiodesk.Trinity;

namespace Artivity.Apid.IO
{
    /// <summary>
    /// A list of of timestamps which represent events for a file.
    /// </summary>
    public class FileEventRecord
    {
        #region Members

        /// <summary>
        /// The time of the event.
        /// </summary>
        public readonly DateTime EventTimeUtc;

        /// <summary>
        /// The current file path in any case.
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// The previous file path in case of a rename or move event.
        /// </summary>
        /// <value>The old file path.</value>
        public readonly string OldFilePath;

        /// <summary>
        /// The URI of the file data object in case of a file create event.
        /// </summary>
        /// <value>The URI.</value>
        public readonly UriRef Uri;

        #endregion

        #region Constructors

        public FileEventRecord(DateTime eventTimeUtc, string filePath, string oldFilePath = null)
        {
            EventTimeUtc = eventTimeUtc;
            FilePath = filePath;
            OldFilePath = oldFilePath;
        }

        public FileEventRecord(DateTime eventTimeUtc, string filePath, UriRef uri)
        {
            EventTimeUtc = eventTimeUtc;
            FilePath = filePath;
            Uri = uri;
        }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }

        #endregion
    }
}
