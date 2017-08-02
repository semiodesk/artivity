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
    public class Entity : SynchronizableResource, IValidatable
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

        [RdfProperty(NAO.hasTag)]
        public List<Tag> Tags { get; set; }

        public IEnumerable<string> RevisionUris
        {
            get { return from a in Revisions select a.Uri.AbsoluteUri; }
        }

		#endregion

		#region Constructors

        public Entity(Uri uri) : base(uri) {}

		#endregion

        #region Methods

        public virtual bool Validate()
        {
            return true;
        }

        public void AddTag(string label)
        {
            if (!Tags.Any(t => t.Label.ToLowerInvariant() == label.ToLowerInvariant()))
            {
                ISparqlQuery query = new SparqlQuery(@"DESCRIBE ?tag WHERE { ?tag nao:prefLabel @label . }");
                query.Bind("@label", label);

                Tag tag = Model.GetResources<Tag>(query).FirstOrDefault();

                if (tag == null)
                {
                    tag = Model.CreateResource<Tag>();
                    tag.Label = label;
                    tag.Commit();
                }

                Tags.Add(tag);
            }
        }

        public void AddTag(UriRef uri, string label = "")
        {
            if (!Tags.Any(t => t.Uri == uri))
            {
                ISparqlQuery query = new SparqlQuery(@"ASK WHERE { @tag a nao:Tag . }");
                query.Bind("@tag", uri);

                Tag tag;

                if(Model.ExecuteQuery(query).GetAnwser())
                {
                    tag = new Tag(uri);
                }
                else
                {
                    tag = Model.CreateResource<Tag>(uri);

                    if(!string.IsNullOrEmpty(label))
                    {
                        tag.Label = label;
                    }

                    tag.Commit();
                }

                Tags.Add(tag);
            }
        }

        public void RemoveTag(string label)
        {
            Tag tag = Tags.FirstOrDefault(t => t.Label.ToLowerInvariant() == label.ToLowerInvariant());

            if (tag != null)
            {
                Tags.Remove(tag);
            }
        }

        public void RemoveTag(UriRef uri)
        {
            Tag tag = Tags.FirstOrDefault(t => t.Uri == uri);

            if (tag != null)
            {
                Tags.Remove(tag);
            }
        }

        #endregion
    }
}

