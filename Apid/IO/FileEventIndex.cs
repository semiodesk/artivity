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
using System.Threading.Tasks;

namespace Artivity.Apid.IO
{
    /// <summary>
    /// An index of file events. Enables quick access to the events via the file name or path.
    /// </summary>
    public class FileEventIndex : Dictionary<string, SortedList<DateTime, FileEventRecord>>
    {
        #region Constructors

        public FileEventIndex()
            : base()
        {
        }

        #endregion

        #region Methods

        public void Add(string key, FileEventRecord value)
        {
            SortedList<DateTime, FileEventRecord> records;

            if (!ContainsKey(key))
            {
                records = new SortedList<DateTime, FileEventRecord>();

                Add(key, records);
            }
            else
            {
                records = this[key];
            }

            if (!records.ContainsKey(value.EventTimeUtc))
            {
                records.Add(value.EventTimeUtc, value);
            }
        }

        public void Remove(string key, FileEventRecord value)
        {
            if (ContainsKey(key))
            {
                SortedList<DateTime, FileEventRecord> records = this[key];

                if (records.ContainsKey(value.EventTimeUtc))
                {
                    records.Remove(value.EventTimeUtc);
                }

                if (records.Count == 0)
                {
                    Remove(key);
                }
            }
        }

        public FileEventRecord TryGetLatestEvent(string key, long fileLength)
        {
            if (ContainsKey(key))
            {
                foreach (KeyValuePair<DateTime, FileEventRecord> item in this[key])
                {
                    FileEventRecord record = item.Value;

                    if (File.Exists(record.FilePath))
                    {
                        FileInfo fileInfo = new FileInfo(record.FilePath);

                        if (fileInfo.Length == fileLength)
                        {
                            return record;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
