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

using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(PROV.Activity)]
    public class Activity : SynchronizableResource, IValidatable
    {
		#region Members

		[RdfProperty(PROV.qualifiedAssociation)]
		public List<Association> Associations { get; private set; }

        [RdfProperty(PROV.qualifiedCommunication)]
        public List<Communication> Communications { get; private set; }

        [RdfProperty(PROV.qualifiedUsage)]
		public List<Usage> Usages { get; private set; }

		[RdfProperty(PROV.invalidated)]
		public List<Entity> InvalidatedEntities { get; set; }

		[RdfProperty(PROV.generated)]
		public List<Entity> GeneratedEntities { get; set; }

        [RdfProperty(PROV.used)]
        public List<Entity> UsedEntities { get; set; }

		[RdfProperty(PROV.startedAtTime)]
		public DateTime StartTime { get; set; }

		[RdfProperty(PROV.endedAtTime)]
		public DateTime EndTime { get; set; }

        [RdfProperty(PROV.wasStartedBy)]
        public Agent StartedBy { get; set; }

		#endregion

        #region Constructors

        public Activity(Uri uri) : base(uri) {}

        #endregion

        #region Methods

        public virtual bool Validate()
        {
            /*
            bool result = Associations.Count > 0 && StartTime > DateTime.MinValue;
            
            if(EndTime > DateTime.MinValue)
            {
                return result & EndTime > StartTime;
            }
            else
            {
                return result;
            }
            */

            if (EndTime > DateTime.MinValue)
            {
                return EndTime > StartTime;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
