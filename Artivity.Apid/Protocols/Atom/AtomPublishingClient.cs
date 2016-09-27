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
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Artivity.Apid.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Artivity.Apid.Protocols.Atom
{
    /// <summary>
    /// A client for publishing files to Atom Publishing Protocol services.
    /// </summary>
    public class AtomPublishingClient
    {
        #region Members

        private readonly Uri _serviceEndpoint;

        private readonly Uri _collectionEndpoint;

        private string _username;

        private string _password;

        #endregion

        #region Constructors

        public AtomPublishingClient(string serviceUrl)
        {
            _serviceEndpoint = new Uri(serviceUrl + "/sword-app/servicedocument");
            _collectionEndpoint = new Uri(serviceUrl + "/id/contents");
        }

        public AtomPublishingClient(Uri serviceUrl)
            : this(serviceUrl.AbsoluteUri)
        {
        }

        #endregion

        #region Methods

        public void SetBasicAuthCredentials(string username, string password)
        {
            _username = username;
            _password = password;
        }

        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(_username + ":" + _password));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            return client;
        }

        public AtomPublishingService TryGetServiceDescription()
        {
            return Task.Run<AtomPublishingService>(() => { return TryGetServiceDescriptionAsync(); }).Result;
        }

        public async Task<AtomPublishingService> TryGetServiceDescriptionAsync()
        {
            using (HttpClient client = GetHttpClient())
            {
                string url = _serviceEndpoint.AbsoluteUri;

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        return AtomPublishingService.TryParse(result);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }

            return null;
        }

        public AtomFeed TryGetUserFeed()
        {
            return Task.Run<AtomFeed>(() => { return TryGetUserFeedAsync(); }).Result;
        }

        public async Task<AtomFeed> TryGetUserFeedAsync()
        {
            using (HttpClient client = GetHttpClient())
            {
                string url = _collectionEndpoint.AbsoluteUri;

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    try
                    {
                        return AtomFeed.TryParse(result);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }

            return null;
        }

        public bool DepositFile(AtomFeedEntry entry, string filePath)
        {
            return Task.Run<bool>(() => { return DepositFileAsync(entry, filePath); }).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://wiki.eprints.org/w/SWORD_2.0"/>
        public async Task<bool> DepositFileAsync(AtomFeedEntry entry, string filePath)
        {
            using (HttpClient client = GetHttpClient())
            {
                string url = _collectionEndpoint.AbsoluteUri;

                FileInfo fileInfo = new FileInfo(filePath);

                if (fileInfo.Exists)
                {
                    MemoryStream stream = new MemoryStream();

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(AtomFeedEntry));

                    using (XmlWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
                    {
                        xmlSerializer.Serialize(xmlWriter, entry);
                    }

                    string atom = Encoding.UTF8.GetString(stream.ToArray());

                    int bufferSize = 4096;

                    ProgressStreamInfo info = new ProgressStreamInfo(atom.Length + fileInfo.Length);

                    ProgressDelegate progressDelegate = (bytes, total, expected) =>
                    {
                        info.TransferredBytes += bytes;

                        RaiseDepositProgressChanged(info);
                    };

                    ProgressStreamContent atomContent = new ProgressStreamContent(atom, bufferSize);
                    atomContent.Headers.ContentType = new MediaTypeHeaderValue("application/atom+xml") { CharSet = "utf-8" };
                    atomContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { Name = "atom" };
                    atomContent.Progress = progressDelegate;

                    ProgressStreamContent fileContent = new ProgressStreamContent(new FileStream(fileInfo.FullName, FileMode.Open), bufferSize);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/illustrator");
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { Name = "payload", FileName = fileInfo.Name };
                    fileContent.Headers.Add("Packaging", sword.NS + "/package/Binary");
                    fileContent.Progress = progressDelegate;

                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Headers.ContentType.MediaType = "multipart/related";
                    data.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/atom+xml\""));
                    data.Add(atomContent);
                    data.Add(fileContent);

                    HttpResponseMessage response = await client.PostAsync(url, data);

                    return response.StatusCode == HttpStatusCode.Created;
                }
            }

            return false;
        }

        #endregion

        #region Events

        public DepositProgressChangedEventHandler DepositProgressChanged;

        private void RaiseDepositProgressChanged(ProgressStreamInfo progressInfo)
        {
            if(DepositProgressChanged != null)
            {
                DepositProgressChanged(this, progressInfo);
            }
        }

        #endregion
    }

    public delegate void DepositProgressChangedEventHandler(object sender, ProgressStreamInfo progressInfo);
}
