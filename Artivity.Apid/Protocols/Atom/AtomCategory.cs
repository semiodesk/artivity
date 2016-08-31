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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// A 'category' in the Atom Publishing Protocol, which is analogoue to a class + label in RDF.
    /// </summary>
    [XmlRoot("category", Namespace=atom.NS)]
    public class AtomCategory
    {
        #region Members

        /// <summary>
        /// Gets or sets a value representing the namespace of the category name / term.
        /// </summary>
        [XmlAttribute("scheme")]
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the category name / term.
        /// </summary>
        [XmlAttribute("term")]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets a human readable name of the category.
        /// </summary>
        [XmlAttribute("label")]
        public string Label { get; set; }

        #endregion

        #region Methods

        internal static AtomCategory FromXElement(XElement e)
        {
            return new AtomCategory
            {
                Scheme = e.GetAttributeValue("scheme", ""),
                Term = e.GetAttributeValue("term", ""),
                Label = e.GetAttributeValue("label", "")
            };
        }

        #endregion
    }
}
