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
    /// Description of an Atom feed entry author.
    /// </summary>
    [XmlRoot("author", Namespace = atom.NS)]
    public class AtomFeedAuthor
    {
        #region Members

        /// <summary>
        /// Gets or sets the author's name.
        /// </summary>
        [XmlElement("name", Namespace = atom.NS)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the author's email address.
        /// </summary>
        [XmlElement("email", Namespace = atom.NS)]
        public string EMail { get; set; }

        #endregion

        #region Methods

        public static AtomFeedAuthor FromXElement(XElement e)
        {
            AtomFeedAuthor result = new AtomFeedAuthor();

            if(e != null)
            {
                result.Name = e.GetElementValue(atom.name, "");
                result.EMail = e.GetElementValue(atom.email, "");
            }

            return result;
        }

        #endregion
    }
}
