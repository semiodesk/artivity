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
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Artivity.Apid.Accounts
{
    /// <summary>
    /// A factory class for creating online service clients.
    /// </summary>
    public static class OnlineServiceClientFactory
    {
        #region Members

        /// <summary>
        /// Indicates if the factory class is initialized.
        /// </summary>
        public static bool IsInitialized = false;

        /// <summary>
        /// A list of registered online service clients.
        /// </summary>
        private static readonly Dictionary<Uri, IOnlineServiceClient> _clients = new Dictionary<Uri, IOnlineServiceClient>();

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the factory class by registering all known online service clients.
        /// </summary>
        public static void Initialize()
        {
            if (IsInitialized) return;

            OnlineServiceClientFactory.RegisterClient(new EPrintsServiceClient());
            OnlineServiceClientFactory.RegisterClient(new OrcidServiceClient());

            IsInitialized = true;
        }

        /// <summary>
        /// Register a new online service client.
        /// </summary>
        /// <param name="client">A online service client.</param>
        public static void RegisterClient(IOnlineServiceClient client)
        {
            Uri uri = client.Uri;

            if(_clients.ContainsKey(client.Uri))
            {
                string message = string.Format("Identifier {0} already registered by client {1}", uri, _clients[uri]);

                throw new DuplicateKeyException(message);
            }

            //PlatformProvider.Logger.LogInfo("Registered online account client {0}", uri);

            _clients[uri] = client;
        }

        /// <summary>
        /// Enumerates all registered online service clients.
        /// </summary>
        public static IEnumerable<IOnlineServiceClient> GetRegisteredClients()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            return _clients.Values;
        }

        /// <summary>
        /// Tries to get a online service client from the 'clientUri' query parameter of a HTTP request.
        /// </summary>
        /// <param name="request">A Nancy HTTP request.</param>
        /// <returns>A online service client on success, <c>null</c> otherwise.</returns>
        public static IOnlineServiceClient TryGetClient(Request request)
        {
            string uri = request.Query.clientUri;

            if(string.IsNullOrEmpty(uri))
            {
                return null;
            }

            return TryGetClient(new Uri(uri));
        }

        /// <summary>
        /// Tries to get a online service client with the given URI.
        /// </summary>
        /// <param name="uri">A URI.</param>
        /// <returns>A online service client on success, <c>null</c> otherwise.</returns>
        public static IOnlineServiceClient TryGetClient(Uri uri)
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            if (_clients.Any(x => x.Key == uri))
            {
                return _clients.FirstOrDefault(x => x.Key == uri).Value;
            }

            return _clients[uri];
        }

        /// <summary>
        /// Tries to get a online service client with the given URI.
        /// </summary>
        /// <typeparam name="T">A subclass of OnlineServiceclientBase.</typeparam>
        /// <param name="uri">A URI.</param>
        /// <returns>A online service client on success, <c>null</c> otherwise.</returns>
        public static T TryGetClient<T>(Uri uri) where T : class
        {
            return TryGetClient(uri) as T;
        }

        #endregion
    }

    public class DuplicateKeyException : Exception
    {
        #region Constructors

        public DuplicateKeyException(string message) : base(message)
        {
        }

        #endregion
    }
}
