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
    /// A 'collections' in the Atom Publishing Protocol, which are analogous to the 'folders' or 'directories' found in many file systems.
    /// </summary>
    public class AtomCollection
    {
        #region Members

        /// <summary>
        /// Gets the collection title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a value indicating if the collection supports SWORD protocol mediation.
        /// </summary>
        /// <seealso href="http://swordapp.github.io/SWORDv2-Profile/SWORDProfile.html"/>
        public bool IsMediationSupported { get; private set; }

        /// <summary>
        /// Gets a list of MIME types which are supported by this collection.
        /// </summary>
        public List<string> SupportedContentTypes { get; private set; }

        /// <summary>
        /// Gets a list of URIs which refer to supported packaging methods.
        /// </summary>
        public List<string> SupportedPackagingTypes { get; private set; }

        /// <summary>
        /// Gets a list of supported content type categories.
        /// </summary>
        public List<AtomCategory> Categories { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of the class.
        /// </summary>
        public AtomCollection()
        {
            Categories = new List<AtomCategory>();
        }

        #endregion

        #region Methods

        internal static AtomCollection FromXElement(XElement e)
        {
            AtomCollection result = new AtomCollection();

            if(e != null)
            {
                result.Title = e.GetElementValue(atom.title, "");
                result.IsMediationSupported = e.GetElementValue(sword.mediation, false);
                result.SupportedContentTypes = e.GetElementValues(app.accept).ToList();
                result.SupportedPackagingTypes = e.GetElementValues(app.acceptPackaging, sword.acceptPackaging).ToList();

                foreach (XElement category in e.Descendants(app.categories))
                {
                    result.Categories.AddRange(category.Elements(atom.category).Select(x => AtomCategory.FromXElement(x)));
                }
            }

            return result;
        }

        #endregion
    }
}
