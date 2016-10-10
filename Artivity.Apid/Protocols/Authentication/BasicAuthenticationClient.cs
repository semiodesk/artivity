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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Protocols.Authentication
{
    public class BasicAuthenticationClient : HttpAuthenticationClientBase
    {
        #region Members

        [JsonIgnore]
        public string Username { get; private set; }

        [JsonIgnore]
        public string Password { get; private set; }

        #endregion

        #region Constructors

        public BasicAuthenticationClient()
            : base("http://localhost:8272/artivity/api/1.0/auth/basic")
        {
            RequiredParameters.Add("url");
            RequiredParameters.Add("username");
            RequiredParameters.Add("password");
        }

        #endregion

        #region Methods

        public override async void HandleRequestAsync(Request request, string sessionId)
        {
            SetRequestStatus(HttpStatusCode.Processing);
            SetRequestParameters(request);

            if(!RequestParameters.IsSupersetOf(RequiredParameters))
            {
                SetRequestStatus(HttpStatusCode.BadRequest);

                return;
            }

            AuthorizeUrl = request.Query.url;

            Username = request.Query.username;
            Password = request.Query.password;

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Username, Password)));

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    HttpResponseMessage response = await client.GetAsync(AuthorizeUrl);

                    SetRequestStatus(response.StatusCode);

                    if (RequestStatus == 200)
                    {
                        ClientState = HttpAuthenticationClientState.Authorized;
                    }
                    else if(RequestStatus == 401)
                    {
                        ClientState = HttpAuthenticationClientState.Unauthorized;
                    }

                    ResponseData = await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                ClientState = HttpAuthenticationClientState.Error;

                Logger.LogError(ex);
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters()
        {
            yield return new KeyValuePair<string,string>("username", Username);
        }

        public override int GetHashCode()
        {
            return GetHashCode() + Username.GetHashCode() + Password.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            BasicAuthenticationClient other = obj as BasicAuthenticationClient;

            if(other == null)
            {
                return false;
            }

            return Username.Equals(other.Username) && Password.Equals(other.Password);
        }

        #endregion
    }
}
