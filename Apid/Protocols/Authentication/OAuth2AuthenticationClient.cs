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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Protocols.Authentication
{
    public class OAuth2AuthenticationClient : HttpAuthenticationClientBase
    {
        #region Members

        protected string RedirectUrl { get; set; }

        protected string TokenUrl { get; set; }

        [JsonIgnore]
        public string ClientId { get; set; }

        [JsonIgnore]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string GrantType { get; set; }

        #endregion

        #region Constructors

        public OAuth2AuthenticationClient()
            : base("http://localhost:8272/artivity/api/1.0/auth/oauth2")
        {
            RequiredParameters = new HashSet<string> { "url" };
        }

        #endregion

        #region Methods

        public override async void HandleRequestAsync(Request request, string sessionId)
        {
            SetRequestStatus(HttpStatusCode.Processing);
            SetRequestParameters(request);

            if (!RequestParameters.IsSupersetOf(RequiredParameters))
            {
                SetRequestStatus(HttpStatusCode.BadRequest);

                return;
            }

            string url = request.Query.url;

            RedirectUrl = string.Format("{0}/artivity/api/1.0/accounts/authorize/oauth2/token?sessionId={1}", request.Url.SiteBase, sessionId);
            AuthorizeUrl = string.Format("{0}/oauth/authorize?client_id={1}&response_type=code&scope=/authenticate&redirect_uri={2}", url, ClientId, RedirectUrl);

            // The site to which the authorization token is being sent can be overriden by using the tokenUrl query parameter.
            string tokenUrl = request.Query.tokenUrl;

            if(!string.IsNullOrEmpty(tokenUrl))
            {
                TokenUrl = tokenUrl;
            }
            else
            {
                TokenUrl = url;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(AuthorizeUrl);

                    // If the HTTP status code is OK, we leave the authenticator in the processing state.
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        SetRequestStatus(response.StatusCode);
                    }

                    ResponseData = await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch(Exception ex)
            {
                ClientState = HttpAuthenticationClientState.Error;

                if( Logger != null)
                    Logger.LogError(ex);
            }
        }

        public void SendAuthorizationToken(Request request, string sessionId)
        {
            if(string.IsNullOrEmpty(request.Query.code))
            {
                SetRequestStatus(HttpStatusCode.BadRequest);
            }

            string code = request.Query.code;

            NameValueCollection content = new NameValueCollection()
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUrl }
            };

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    client.UploadValuesCompleted += (object sender, System.Net.UploadValuesCompletedEventArgs e) =>
                    {
                        ResponseData = e.Result;

                        if (e.Error == null)
                        {
                            if (Logger != null)
                                Logger.LogInfo("OAuth 2.0 authorization success.");

                            ClientState = HttpAuthenticationClientState.Authorized;

                            SetRequestStatus(HttpStatusCode.OK);
                        }
                        else
                        {
                            if (Logger != null)
                                Logger.LogError("OAuth 2.0 authorization failed: {0}", e.Error.Message);

                            ClientState = HttpAuthenticationClientState.Error;

                            SetRequestStatus(HttpStatusCode.Unauthorized);
                        }
                    };

                    client.UploadValuesAsync(new Uri(TokenUrl), "POST", content);
                }
            }
            catch (Exception ex)
            {
                ClientState = HttpAuthenticationClientState.Error;

                if (Logger != null)
                    Logger.LogError(ex);
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters()
        {
            yield break;
        }

        #endregion
    }
}
