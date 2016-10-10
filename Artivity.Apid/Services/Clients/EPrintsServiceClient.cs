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

using Artivity.Apid.IO;
using Artivity.Apid.Protocols.Atom;
using Artivity.Apid.Protocols.Authentication;
using Artivity.DataModel;
using Nancy;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A helper class for installing an authenticated EPrints online account.
    /// </summary>
    public class EPrintsServiceClient : OnlineServiceClientBase, IOnlineServicePublishingClient
    {
        #region Constructors

        public EPrintsServiceClient() : base(new Uri("http://eprints.org"))
        {
            Title = "EPrints";

            HttpAuthenticationParameterSet localhost = new HttpAuthenticationParameterSet();
            localhost.Id = "localhost:8080";
            localhost.Parameters["authType"] = "http://localhost:8272/artivity/api/1.0/auth/basic";
            localhost.Parameters["url"] = "http://localhost:8080/id/contents";

            Presets.Add(localhost);

            Features.Add(artf.PublishArchive);

            SupportedAuthenticationClients.Add(new BasicAuthenticationClient());
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

            // Issue the request against the EPrints specific /id/contents path.
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
        protected override UriRef GetAccountUri()
        {
            BasicAuthenticationClient authenticator = TryGetAuthenticationClient<BasicAuthenticationClient>(HttpAuthenticationClientState.Authorized);

            if (authenticator != null)
            {
                UriBuilder uriBuilder = new UriBuilder(ServiceUrl);
                uriBuilder.UserName = authenticator.Username;
                uriBuilder.Path = uriBuilder.Path.TrimEnd('/') + "/id/contents";

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
            return "EPrints";
        }

        public void UploadArchive(Request request, Uri serviceUrl, string filePath, ArchiveManifest manifest = null)
        {
            string username = request.Query.username;
            string password = request.Query.password;

            AtomPublishingClient client = new AtomPublishingClient(serviceUrl);
            client.SetBasicAuthCredentials(username, password);

            AtomFeedEntry entry = new AtomFeedEntry();
            entry.Type = "dataset";
            entry.Subjects.Add("N1");
            entry.Subjects.Add("NX");

            if(manifest != null)
            {
                entry.Title = manifest.Title;
                entry.Description = manifest.Description;

                foreach(ArchiveManifestCreator creator in manifest.Creators)
                {
                    AtomFeedAuthor author = new AtomFeedAuthor()
                    {
                        Name = creator.Name,
                        EMail = creator.EmailAddress
                    };

                    entry.Authors.Add(author);
                }

                if(Uri.IsWellFormedUriString(manifest.License, UriKind.Absolute))
                {
                    entry.License = new AtomLink("license", new Uri(manifest.License));
                }
            }
            else
            {
                entry.Title = Path.GetFileName(filePath);
            }

            // The progress of the client is the current operation.
            Progress.Id = client.Progress.Id;

            client.ProgressChanged += (sender, progress) =>
            {
                Progress.Total = progress.Total;
                Progress.Completed = progress.Completed;
            };

            client.DepositFile(entry, filePath);
        }

        #endregion
    }
}
