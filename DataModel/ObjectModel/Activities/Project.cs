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
using System.Collections.Generic;
using System.Linq;
using Semiodesk.Trinity;
using Newtonsoft.Json;

namespace Artivity.DataModel
{
    [RdfClass(ART.Project)]
    public class Project : Activity
    {
        #region Members

        [RdfProperty(DCES.title)]
        public string Title { get; set; }

        [RdfProperty(DCES.description)]
        public string Description { get; set; }

        [RdfProperty(ART.colorCode)]
        public string ColorCode { get; set; }

        [RdfProperty(ART.qualifiedMembership)]
        public List<ProjectMembership> Memberships { get; set; }

        [JsonIgnore]
        public IEnumerable<Agent> Members
        {
            get
            {
                foreach (ProjectMembership membership in Memberships)
                {
                    yield return membership.Agent;
                }
            }
        }

        [JsonIgnore]
        public IEnumerable<Entity> UsedEntities
        {
            get
            {
                foreach(Usage usage in Usages)
                {
                    yield return usage.Entity;
                }
            }
        }

        #endregion

        #region Constructors

        public Project(Uri uri) : base(uri)
        {
            IsSynchronizable = true;
        }

        #endregion
    }
}

