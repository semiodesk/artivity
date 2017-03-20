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

using System;
using Semiodesk.Trinity;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Artivity.DataModel
{
	[RdfClass(NFO.FileDataObject)]
	public class FileDataObject : SynchronizableResource
	{
		#region Members

        [RdfProperty(RDFS.label), JsonIgnore]
        public string Name { get; set; }

		[RdfProperty(NFO.fileSize), JsonIgnore]
		public long ByteSize { get; set; }

		[RdfProperty(NFO.fileLastAccessed), JsonIgnore]
		public DateTime LastAccessTimeUtc { get; set; }

        [RdfProperty(NFO.deletionDate), JsonIgnore]
        public DateTime? FileDeletionTimeUtc { get; set; }

        [RdfProperty(NFO.belongsToContainer), JsonIgnore]
        public Folder Folder { get; set; }

		#endregion

		#region Constructors

		public FileDataObject(Uri uri) : base(uri) 
        {
            IsSynchronizable = true;
        }

        public FileDataObject(string uri) : base(uri)
        {
            IsSynchronizable = true;
        }

		#endregion
	}
}

