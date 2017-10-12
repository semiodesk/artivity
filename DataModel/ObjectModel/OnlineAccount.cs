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

using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(FOAF.OnlineAccount)]
    public class OnlineAccount : Resource, IValidatable
    {
        #region Members

        /// <summary>
        /// Indicates the identifier associated with this online account.
        /// </summary>
        /// <example>
        /// <foaf:accountName>jwales</foaf:accountName>
        /// </example>
        [RdfProperty(FOAF.accountName)]
        public string Id { get; set; }

        [RdfProperty(FOAF.title)]
        public string Title { get; set; }

        [RdfProperty(FOAF.description)]
        public string Description { get; set; }

        [RdfProperty(NIE.created)]
        public DateTime CreationTime { get; set; }

        [RdfProperty(NIE.lastModified)]
        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// The client URI for the service.
        /// </summary>
        [RdfProperty(ART.accountServiceClient)]
        public Resource ServiceClient { get; set; }

        /// <summary>
        /// Indicates a homepage of the service provide for this online account.
        /// </summary>
        /// <example>
        /// <foaf:accountServiceHomepage rdf:resource="http://www.freenode.net/"/>
        /// </example>
        [RdfProperty(FOAF.accountServiceHomepage)]
        public Resource ServiceUrl { get; set; }

        [RdfProperty(ART.authenticationProtocol)]
        public HttpAuthenticationProtocol AuthenticationProtocol { get; set; }

        [RdfProperty(ART.authenticationParameter)]
        public List<HttpAuthenticationParameter> AuthenticationParameters { get; set; }

        #endregion

        #region Constructors

        public OnlineAccount(Uri uri) : base(uri) { }

        #endregion

        #region Members

        public string GetParameter(string name)
        {
            HttpAuthenticationParameter parameter = AuthenticationParameters.FirstOrDefault(p => p.Name == name);

            return parameter != null ? parameter.Value : null;
        }

        public void SetParameter(string name, string value)
        {
            HttpAuthenticationParameter parameter = AuthenticationParameters.FirstOrDefault(p => p.Name == name);

            if (parameter != null)
            {
                parameter.Value = value;
                parameter.Commit();
            }
            else
            {
                parameter = this.Model.CreateResource<HttpAuthenticationParameter>();
                parameter.Name = name;
                parameter.Value = value;
                AuthenticationParameters.Add(parameter);
                Commit();
            }

        }

        public virtual bool Validate()
        {
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Title) && ServiceUrl != null && ServiceClient != null;
        }

        #endregion
    }
}
