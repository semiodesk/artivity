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

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// Description of a Atom Publishing Protocol service and its features.
    /// </summary>
    public class AtomPublishingService
    {
        #region Members

        /// <summary>
        /// Get the title of the service.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Get the supported SWORD protocol version number.
        /// </summary>
        public Version ProtocolVersion { get; private set; }

        /// <summary>
        /// Get a description of the service feeds.
        /// </summary>
        public AtomCollection Collection { get; private set; }

        #endregion

        #region Methods

        public static AtomPublishingService TryParse(string xml)
        {
            IEnumerable<XElement> services = XDocumentFactory.ParseUnicode(xml)
                .Descendants(app.service)
                .Descendants(app.workspace);
            
            if(services.Any())
            {
                return AtomPublishingService.FromXElement(services.First());
            }

            return null;
        }

        internal static AtomPublishingService FromXElement(XElement e)
        {
            AtomPublishingService result = new AtomPublishingService();

            if (e != null)
            {
                result.Title = e.GetElementValue(atom.title, "");
                result.ProtocolVersion = e.GetElementValue(sword.version, new Version());

                if (e.Elements(app.collection).Any())
                {
                    result.Collection = AtomCollection.FromXElement(e.Elements(app.collection).First());
                }
                else
                {
                    result.Collection = new AtomCollection();
                }
            }

            return result;
        }

        #endregion
    }
}
