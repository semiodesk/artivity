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
// Copyright (c) Semiodesk GmbH 2017

using Artivity.DataModel.ObjectModel;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.DataModel
{
    [RdfClass(RDFS.Resource)]
    public class SynchronizableResource : Resource
    {
        #region Members

        protected bool IsSynchronizable;

        [RdfProperty(NIE.created)]
        public DateTime CreationTimeUtc { get; set; }

        [RdfProperty(NIE.lastModified)]
        public DateTime ModificationTimeUtc { get; set; }

        [RdfProperty(ART.deleted)]
        public DateTime DeletionTimeUtc { get; set; }

        [RdfProperty(ARTS.synchronizationState), JsonIgnore]
        public ResourceSynchronizationState SynchronizationState { get; set; }

        #endregion

        #region Constructors

        public SynchronizableResource(Uri uri) : base(uri)
        {
        }

        #endregion

        #region Methods

        public void Sanitize()
        {
            List<object> values = ListValues(arts.synchronizationState).ToList();

            if(values.Count > 1)
            {
                for(int i = 1; i < values.Count; i++)
                {
                    Resource r = values[i] as Resource;

                    if(r != null)
                    {
                        ResourceSynchronizationState s = new ResourceSynchronizationState(r.Uri);

                        RemoveProperty(arts.synchronizationState, s);

                        if (Model.ContainsResource(r.Uri))
                        {
                            Model.DeleteResource(r.Uri);
                        }
                    }
                }

                base.Commit();
                base.Rollback();
            }
        }

        public override void Commit()
        {
            Commit(-1);
        }

        public void Commit(int revision)
        {
            if (IsNew || CreationTimeUtc == DateTime.MinValue)
            {
                CreationTimeUtc = DateTime.UtcNow;
                DeletionTimeUtc = DateTime.MinValue;
            }

            ModificationTimeUtc = DateTime.UtcNow;

            if (IsSynchronizable)
            {
                try
                {
                    if (SynchronizationState == null)
                    {
                        SynchronizationState = Model.CreateResource<ResourceSynchronizationState>();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Caught exception when trying to access sync state of resource {1}", Uri);

                    Sanitize();
                }

                if (SynchronizationState != null)
                {
                    // The resource is flagged as modified. The account synchronizer will update 
                    // the last update counter and the flag when the resource was successfully uploaded.
                    SynchronizationState.LastRemoteRevision = revision;
                    SynchronizationState.Commit();
                }
            }

            base.Commit();
        }

        #endregion
    }
}
