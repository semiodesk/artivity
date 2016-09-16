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

using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public abstract class OAuth2AccountProvider : OnlineAccountProvider, IOAuth2AccountProvider
    {
        #region Members

        [JsonIgnore]
        public string ClientId { get; set; }

        [JsonIgnore]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string GrantType { get; set; }

        protected string RedirectUrl { get; set; }

        protected byte[] ResponseData;

        #endregion

        #region Constructors

        public OAuth2AccountProvider(Uri uri) : base(uri)
        {
        }

        #endregion

        #region Methods

        public abstract string GetAuthorizationRequestUrl(string redirectUrl);

        public virtual void Authorize(IModel model, string code)
        {
            if(model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("OAuth2 authorization code must not be null or empty.");
            }
        }

        protected void UploadValues(IModel model, Uri url, NameValueCollection data)
        {
            using (WebClient client = new WebClient())
            {
                client.UploadValuesCompleted += (object sender, UploadValuesCompletedEventArgs e) =>
                {
                    if (e.Error != null)
                    {
                        ResponseData = null;

                        Logger.LogError("OAuth2 authorization failed: {0}", e.Error.Message);
                    }
                    else
                    {
                        ResponseData = e.Result;

                        Logger.LogInfo("OAuth2 authorization success.");

                        TryCreateAccount(model);
                    }
                };

                client.UploadValuesAsync(url, "POST", data);
            }
        }

        #endregion
    }
}
