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

using Artivity.Apid.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// An Atom feed entry.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/Atom_(standard)"/>
    [XmlRoot("entry", Namespace=atom.NS)]
    public class AtomFeedEntry
    {
        #region Members

        /// <summary>
        /// Gets a value which refers to the entry ID in the service it was retrieved from.
        /// </summary>
        [XmlIgnore]
        [XmlElement("id", Namespace=atom.NS)]
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the title of the entry.
        /// </summary>
        [XmlElement("title", Namespace=atom.NS)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the entry contents.
        /// </summary>
        [XmlElement("summary", Namespace = atom.NS)]
        public string Description { get; set; }

        /// <summary>
        /// Gets the entry publishing state defined by the SWORD protocol.
        /// </summary>
        /// <seealso href="http://swordapp.github.io/SWORDv2-Profile/SWORDProfile.html"/>
        [XmlIgnore]
        [XmlElement("state", Namespace = sword.NS)]
        public string State { get; private set; }

        /// <summary>
        /// Gets the time the entry was published.
        /// </summary>
        [XmlIgnore]
        [XmlElement("published", Namespace = atom.NS)]
        public DateTime PublicationTimeUtc { get; private set; }

        /// <summary>
        /// Gets the time the entry was last updated.
        /// </summary>
        [XmlIgnore]
        [XmlElement("updated", Namespace = atom.NS)]
        public DateTime LastUpdateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the authors of the entry.
        /// </summary>
        [XmlElement("author", Namespace = atom.NS)]
        public List<AtomFeedAuthor> Authors { get; set; }

        /// <summary>
        /// Gets or sets the categories of the entry.
        /// </summary>
        [XmlElement("category", Namespace = atom.NS)]
        public List<AtomCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the categories of the entry.
        /// </summary>
        [XmlElement("type", Namespace = dcterms.NS)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the subjects / topics of the entry.
        /// </summary>
        [XmlElement("subject", Namespace=dcterms.NS)]
        public List<string> Subjects = new List<string>();

        #endregion

        #region Constructors

        public AtomFeedEntry()
        {
            Authors = new List<AtomFeedAuthor>();
            Categories = new List<AtomCategory>();
        }

        #endregion

        #region Methods

        public static AtomFeedEntry TryParse(string xml)
        {
            foreach (XElement e in XDocumentFactory.ParseUnicode(xml).Descendants(atom.entry))
            {
                return AtomFeedEntry.FromXElement(e);
            }

            return null;
        }

        internal static AtomFeedEntry FromXElement(XElement e)
        {
            AtomFeedEntry result = new AtomFeedEntry();

            if(e != null)
            {
                result.Id = e.GetElementValue(atom.id, "-1");
                result.Title = e.GetElementValue(atom.title, "");
                result.Description = e.GetElementValue(atom.summary, "");
                result.State = e.GetElementValue(sword.state, "");
                result.PublicationTimeUtc = e.GetElementValueUtc(atom.published, DateTime.MinValue);
                result.LastUpdateTimeUtc = e.GetElementValueUtc(atom.updated, DateTime.MinValue);
                result.Authors.AddRange(e.Elements(atom.author).Select(x => AtomFeedAuthor.FromXElement(x)));
                result.Categories.AddRange(e.Elements(atom.category).Select(x => AtomCategory.FromXElement(x)));
            }

            return result;
        }

        #endregion
    }
}
