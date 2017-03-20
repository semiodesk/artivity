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

using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Artivity.DataModel
{
	[RdfClass(PROV.Entity)]
    public class Entity : SynchronizableResource
	{
		#region Members

		[RdfProperty(PROV.specializationOf), JsonIgnore]
		public Entity GenericEntity { get; set; }

		[RdfProperty(PROV.wasRevisionOf), JsonIgnore]
		public Entity RevisedEntity { get; set; }

        [RdfProperty(PROV.hadPrimarySource), JsonIgnore]
        public Entity PrimarySource { get; set; }

        [RdfProperty(PROV.qualifiedRevision), JsonIgnore]
        public List<Revision> Revisions { get; set; }

        [RdfProperty(ART.publish), JsonIgnore]
        public bool Publish { get; set; }

        public IEnumerable<string> RevisionUris
        {
            get { return from a in Revisions select a.Uri.AbsoluteUri; }
        }

		#endregion

		#region Constructors

        public Entity(Uri uri) : base(uri) {}

		#endregion
    }
}

