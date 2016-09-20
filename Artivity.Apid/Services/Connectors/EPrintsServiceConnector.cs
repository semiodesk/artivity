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

using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A helper class for installing an authenticated EPrints online account.
    /// </summary>
    public class EPrintsServiceConnector : OnlineServiceConnectorBase
    {
        #region Constructors

        public EPrintsServiceConnector() : base(new Uri("http://eprints.org"))
        {
            Title = "EPrints";

            HttpAuthenticationParameterSet localhost = new HttpAuthenticationParameterSet();
            localhost.Id = "localhost:8080";
            localhost.Parameters["authType"] = "http://localhost:8272/artivity/api/1.0/auth/basic";
            localhost.Parameters["url"] = "http://localhost:8080/id/contents";

            Presets.Add(localhost);

            BasicAuthenticationClient basic = new BasicAuthenticationClient();

            AuthenticationClients.Add(basic);
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

            if(!url.EndsWith("/id/contents"))
            {
                url.TrimEnd('/');
                url += "/id/contents";

                request.Query.url = url;
            }
        }

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override string GetAccountId()
        {
            BasicAuthenticationClient authenticator = TryGetAuthenticationClient<BasicAuthenticationClient>(HttpAuthenticationClientState.Authorized);

            if (authenticator != null)
            {
                return authenticator.Username;
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
            return "EPrints";
        }

        #endregion
    }
}
