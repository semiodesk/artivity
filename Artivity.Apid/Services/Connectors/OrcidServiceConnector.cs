﻿// LICENSE:
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
    public class OrcidServiceConnector : OnlineServiceConnectorBase
    {
        #region Constructors

        public OrcidServiceConnector()
            : base(new Uri("http://orcid.org"))
        {
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
            oauth2.ClientId = "APP-AF3XVP6X01AMZQH1";
            oauth2.ClientSecret = "0c883177-cfd8-4a0f-a111-03a5ba19539d";

            AuthenticationClients.Add(oauth2);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the account identifier.
        /// </summary>
        /// <remarks>
        /// This method is called when an authenticated account is being persisted.
        /// </remarks>
        protected override string GetAccountId()
        {
            IHttpAuthenticationClient authenticator = TryGetAuthenticationClient(HttpAuthenticationClientState.Authorized);

            if (authenticator != null && authenticator.ResponseData != null)
            {
                string json = Encoding.UTF8.GetString(authenticator.ResponseData);

                JObject response = JObject.Parse(json);

                return response["orcid"].ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets the account identifier.
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