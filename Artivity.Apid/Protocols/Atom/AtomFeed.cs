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

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// A feed which contains entries, which may be headlines, full-text articles, excerpts, summaries, 
    /// and/or links to content on a website, along with various metadata.
    /// </summary>
    public class AtomFeed
    {
        #region Members

        /// <summary>
        /// Gets a value which refers to the feed ID in the service it was retrieved from.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the title of the feed.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the time feed was last updated.
        /// </summary>
        public DateTime LastUpdateTimeUtc { get; private set; }

        /// <summary>
        /// Gets the feed entries.
        /// </summary>
        public List<AtomFeedEntry> Entries { get; private set; }

        #endregion

        #region Constructors

        public AtomFeed()
        {
            Entries = new List<AtomFeedEntry>();
        }

        #endregion

        #region Methods

        public static AtomFeed TryParse(string xml)
        {
            foreach (XElement e in XDocumentFactory.ParseUnicode(xml).Descendants(atom.feed))
            {
                return AtomFeed.FromXElement(e);
            }

            return null;
        }

        internal static AtomFeed FromXElement(XElement e)
        {
            AtomFeed result = new AtomFeed();

            if(e != null)
            {
                result.Id = e.GetElementValue(atom.id, "-1");
                result.Title = e.GetElementValue(atom.title, "");
                result.LastUpdateTimeUtc = e.GetElementValueUtc(atom.updated, DateTime.MinValue);
                result.Entries.AddRange(e.Elements(atom.entry).Select(x => AtomFeedEntry.FromXElement(x)));
            }

            return result;
        }

        #endregion
    }
}
