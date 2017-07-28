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

        [RdfProperty(NIE.created)]
        public DateTime CreationTimeUtc { get; set; }

        [RdfProperty(NIE.lastModified)]
        public DateTime ModificationTimeUtc { get; set; }

        [RdfProperty(ART.deleted)]
        public DateTime DeletionTimeUtc { get; set; }

        [RdfProperty(ARTS.synchronizationEnabled)]
        public bool IsSynchronizationEnabled { get; set; }

        [RdfProperty(ARTS.synchronizationState), JsonIgnore]
        public ResourceSynchronizationState SynchronizationState { get; set; }

        #endregion

        #region Constructors

        public SynchronizableResource(Uri uri) : base(uri) { }

        public SynchronizableResource(string uri) : base(uri) { }

        #endregion

        #region Methods

        public static T FromJson<T>(string json) where T : SynchronizableResource
        {
            ResourceResolver resolver = new ResourceResolver();
            resolver.IngoreTypes = new Type[] { typeof(Activity) };

            // Note: If you are having issues where JSON.NET cannot find a public default 
            // constructor or one parameterized constructor, then you are probably having 
            // more than one parameterized constructor.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = resolver;

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public override void Commit()
        {
            Commit(-1);
        }

        public void Commit(int localRevision, int remoteRevision = -1)
        {
            if (this.Model != null)
            {
                if (IsNew || CreationTimeUtc == DateTime.MinValue)
                {
                    CreationTimeUtc = DateTime.UtcNow;
                    DeletionTimeUtc = DateTime.MinValue;
                }

                ModificationTimeUtc = DateTime.UtcNow;

                bool error = false;

                if (IsSynchronizationEnabled)
                {
                    ResourceSynchronizationState state = null;

                    try
                    {
                        // This will get the sync state from the resource. If there exists more than one, which 
                        // may occasionally happen when syncing with Artivity Online. This will throw an exception.
                        state = SynchronizationState;

                        if (state == null)
                        {
                            state = Model.CreateResource<ResourceSynchronizationState>();

                            SynchronizationState = state;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Exception when trying to access sync state of: {0}", Uri);

                        error = true;

                        // 1. Delete all resource synchronization states.
                        DeleteSynchronizationStates();

                        // 2. Create a clean synchronization state and set it.
                        state = Model.CreateResource<ResourceSynchronizationState>();

                        SynchronizationState = state;
                    }

                    if (state != null)
                    {
                        // The resource is flagged as modified. The account synchronizer will update 
                        // the last update counter and the flag when the resource was successfully uploaded.
                        state.LastRemoteRevision = remoteRevision;
                        state.LastLocalRevision = localRevision;
                        state.Commit();
                    }
                }

                base.Commit();

                if (error)
                {
                    // After commit, reload the ResourceCache for the mapped properties and fill it with the sanitized values.
                    base.Rollback();
                }
            }
        }

        private void DeleteSynchronizationStates()
        {
            SparqlUpdate update = new SparqlUpdate(@"
                WITH @model
                DELETE { ?s ?p ?o . }
                WHERE
                {
                    @resource arts:synchronizationState ?s .

                    ?s a arts:ResourceSynchronizationState ; ?p ?o .
                }
                DELETE
                WHERE { @resource arts:synchronizationState ?s . }
            ");

            update.Bind("@model", this.Model);
            update.Bind("@resource", this);

            Model.ExecuteUpdate(update);
        }

        #endregion
    }
}
