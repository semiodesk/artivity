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

namespace Artivity.DataModel
{
	[RdfClass(NFO.FileDataObject)]
	public class FileDataObject : Entity
	{
		#region Members

        [RdfProperty(RDFS.label)]
        public string Name { get; set; }

		[RdfProperty(NFO.fileSize)]
		public long ByteSize { get; set; }

		[RdfProperty(NIE.created)]
		public DateTime CreationTimeUtc { get; set; }

		[RdfProperty(NFO.fileLastAccessed)]
		public DateTime LastAccessTimeUtc { get; set; }

		[RdfProperty(NIE.lastModified)]
		public DateTime LastModificationTimeUtc { get; set; }

        [RdfProperty(NFO.deletionDate)]
        public DateTime? DeletionTimeUtc { get; set; }

        [RdfProperty(NFO.belongsToContainer)]
        public Folder Folder { get; set; }

		#endregion

		#region Constructors

		public FileDataObject(Uri uri) : base(uri) {}

		#endregion
	}
}

