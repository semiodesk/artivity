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
    public class OrcidAccountProvider : OAuth2AccountProvider
    {
        #region Constructors

        public OrcidAccountProvider()
            : base(new Uri("http://orcid.org"))
        {
            SupportedAuthenticationMethods = new List<IOnlineAccountAuthenticator>()
            {
                new HttpOAuth2Authenticator() {
                    ClientId = "APP-AF3XVP6X01AMZQH1",
                    ClientSecret = "0c883177-cfd8-4a0f-a111-03a5ba19539d"
                }
            };
        }

        #endregion

        #region Methods

        public override string GetAuthorizationRequestUrl(string redirectUrl)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                Logger.LogError("OAuth2 redirect URL is empty.");

                return string.Empty;
            }

            RedirectUrl = redirectUrl;

            string url = "https://sandbox.orcid.org/oauth/authorize?client_id={0}&response_type=code&scope=/authenticate&redirect_uri={1}";

            return string.Format(url, ClientId, RedirectUrl);
        }

        public override void Authorize(IModel model, string code)
        {
            base.Authorize(model, code);

            Uri url = new Uri("https://pub.sandbox.orcid.org/oauth/token");

            NameValueCollection values = new NameValueCollection()
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUrl }
            };

            UploadValues(model, url, values);
        }

        protected override OnlineAccount OnCreateAccount(IModel model)
        {
            if(ResponseData == null)
            {
                Logger.LogError("Cannot create account because response data is null.");

                return null;
            }

            DateTime now = DateTime.UtcNow;

            string json = Encoding.UTF8.GetString(ResponseData);

            JObject response = JObject.Parse(json);

            OnlineAccount account = model.CreateResource<OnlineAccount>();
            account.Id = response["orcid"].ToString();
            account.CreationTime = now;
            account.LastModificationTime = now;

            return account;
        }

        #endregion
    }
}
