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

using Artivity.Api;
using Artivity.Apid.Accounts;
using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Protocols.Authentication
{
    public abstract class HttpAuthenticationClientBase : IHttpAuthenticationClient
    {
        #region Members

        /// <summary>
        /// Gets the URI of the authenticator.
        /// </summary>
        public Uri Uri { get; protected set; }

        /// <summary>
        /// Gets the URL which should be used to start the authorization.
        /// </summary>
        public string AuthorizeUrl { get; protected set; }

        /// <summary>
        /// Gets a list of required GET parameters which need to be provided when starting a new authentication session.
        /// </summary>
        public HashSet<string> RequiredParameters { get; protected set; }

        public HashSet<string> RequestParameters { get; protected set; }

        public int RequestStatus { get; protected set; }

        [JsonIgnore]
        public byte[] ResponseData { get; protected set; }

        public HttpAuthenticationClientState ClientState { get; protected set; }

        [JsonIgnore]
        public ILogger Logger { get; set; }

        #endregion

        #region Constructors

        public HttpAuthenticationClientBase(string uri)
        {
            Logger = new Logger();
            Uri = new Uri(uri);
            RequestStatus = 0;
            RequestParameters = new HashSet<string>();
            RequiredParameters = new HashSet<string>();
        }

        #endregion

        #region Methods

        protected void SetRequestStatus(Nancy.HttpStatusCode statusCode)
        {
            SetRequestStatus(Convert.ToInt32(statusCode));
        }

        protected void SetRequestStatus(System.Net.HttpStatusCode statusCode)
        {
            SetRequestStatus(Convert.ToInt32(statusCode));
        }

        private void SetRequestStatus(int statusCode)
        {
            RequestStatus = statusCode;

            if(statusCode == 102)
            {
                ClientState = HttpAuthenticationClientState.Processing;
            }
            else if(statusCode >= 400)
            {
                ClientState = HttpAuthenticationClientState.Error;
            }
        }

        protected void SetRequestParameters(Request request)
        {
            foreach (string key in request.Query.GetDynamicMemberNames())
            {
                RequestParameters.Add(key);
            }
        }

        public abstract void HandleRequestAsync(Request request, string token);

        public abstract IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters();

        #endregion
    }
}
