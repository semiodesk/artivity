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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// An Atom link.
    /// </summary>
    /// <seealso cref="https://en.wikipedia.org/wiki/Atom_(standard)"/>
    [XmlRoot("link", Namespace = atom.NS)]
    public class AtomLink
    {
        #region Members

        [XmlAttribute("rel")]
        public string Relation { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("href")]
        public string Url { get; set; }

        #endregion

        #region Constructors

        public AtomLink()
        {
        }

        public AtomLink(string relation, Uri url, string type = "")
        {
            Relation = relation;
            Url = url.OriginalString;

            if(!string.IsNullOrEmpty(type))
            {
                Type = type;
            }
        }

        #endregion

        #region Methods

        public bool ShouldSerializeType()
        {
            return !string.IsNullOrEmpty(Type);
        }

        public bool ShouldSerializeUrl()
        {
            return !string.IsNullOrEmpty(Url);
        }

        public static AtomLink TryParse(string xml)
        {
            foreach (XElement e in XDocumentFactory.ParseUnicode(xml).Descendants(atom.link))
            {
                return AtomLink.FromXElement(e);
            }

            return null;
        }

        internal static AtomLink FromXElement(XElement e)
        {
            AtomLink result = new AtomLink();

            if (e != null)
            {
                result.Relation = e.GetAttributeValue(atom.rel, "");
                result.Type = e.GetAttributeValue(atom.type, "");
                result.Url = e.GetAttributeValue(atom.href, "");
            }

            return result;
        }

        #endregion
    }
}
