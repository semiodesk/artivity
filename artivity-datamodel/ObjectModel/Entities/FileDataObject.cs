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

namespace Artivity.DataModel
{
	[RdfClass(NFO.FileDataObject)]
	public class FileDataObject : Entity
	{
		#region Members

        // TODO: Map into a URI, not a literal value. (Needs changing libartivity).
		[RdfProperty(NFO.fileUrl)]
		public string Url { get; set; }

		[RdfProperty(NFO.fileSize)]
		public long ByteSize { get; set; }

		[RdfProperty(NFO.fileCreated)]
		public DateTime CreationTime { get; set; }

		[RdfProperty(NFO.fileLastAccessed)]
		public DateTime LastAccessTime { get; set; }

		[RdfProperty(NFO.fileLastModified)]
		public DateTime LastModificationTime { get; set; }

		[RdfProperty(PROV.value)]
		public string RevisedValue { get; set; }

        [RdfProperty(ART.canvas)]
        public Canvas Canvas { get; set; }

		#endregion

		#region Constructors

		public FileDataObject(Uri uri) : base(uri) {}

		#endregion
	}
}

