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

using Artivity.Api.Platform;
using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A helper class for installing an authenticated ORCiD online account.
    /// </summary>
    public class OrcidServiceClient : OnlineServiceClientBase
    {
        #region Constructors

        public OrcidServiceClient(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base(new UriRef("http://orcid.org"), modelProvider, platformProvider)
        {
            Title = "ORCiD";

            HttpAuthenticationParameterSet orcid = new HttpAuthenticationParameterSet();
            orcid.Id = "orcid.org";
            orcid.Parameters["authType"] = "http://localhost:8272/artivity/api/1.0/auth/oauth2";
            orcid.Parameters["url"] = "http://orcid.org";
            orcid.Parameters["tokenUrl"] = "https://pub.orcid.org/oauth/token";

            HttpAuthenticationParameterSet sandbox = new HttpAuthenticationParameterSet();
            sandbox.Id = "sandbox.orcid.org";
            sandbox.Parameters["authType"] = "http://localhost:8272/artivity/api/1.0/auth/oauth2";
            sandbox.Parameters["url"] = "http://sandbox.orcid.org";
            sandbox.Parameters["tokenUrl"] = "https://pub.sandbox.orcid.org/oauth/token";

            Presets.Add(orcid);
            Presets.Add(sandbox);

            OAuth2AuthenticationClient oauth2 = new OAuth2AuthenticationClient();
            oauth2.ClientId = "APP-5JQIA2KPUCN4Z29F";
            oauth2.ClientSecret = "86c6b72d-dfb5-420f-b963-838fcc2daa20";

            SupportedAuthenticationClients.Add(oauth2);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the provides HTTP request query parameters are sane and may correct errors.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        public override void SanitizeQueryParameters(Request request)
        {
            base.SanitizeQueryParameters(request);

            string url = request.Query.url;

            // Save the service endpoint address.
            ServiceUrl = new Uri(url);
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override UriRef GetAccountUri()
        {
            IHttpAuthenticationClient authenticator = TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator != null && authenticator.ResponseData != null)
            {
                string json = Encoding.UTF8.GetString(authenticator.ResponseData);

                JObject response = JObject.Parse(json);

                string id = response["orcid"].ToString();

                UriBuilder uriBuilder = new UriBuilder(ServiceUrl);
                uriBuilder.Path += id;

                return new UriRef(uriBuilder.ToString());
            }

            return null;
        }

        /// <summary>
        /// Gets the account title.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override string GetAccountTitle()
        {
            return "ORCiD";
        }

        #endregion
    }
}
