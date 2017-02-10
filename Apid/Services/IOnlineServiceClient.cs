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

namespace Artivity.Apid
{
    /// <summary>
    /// A helper class for installing authenticated online accounts.
    /// </summary>
    public interface IOnlineServiceClient
    {
        #region Members

        /// <summary>
        /// Gets the Uniform Resource Identifier.
        /// </summary>
        UriRef Uri { get; }

        /// <summary>
        /// Gets a list of features which are supported by the client.
        /// </summary>
        List<Resource> ClientFeatures { get;  }

        /// <summary>
        /// Gets a list of supported HTTP authentication methods.
        /// </summary>
        List<IHttpAuthenticationClient> SupportedAuthenticationClients { get; }

        /// <summary>
        /// Get information about the current client operations.
        /// </summary>
        object TaskState { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializeses additional HTTP request query parameters from a preset ID which is provided via a query parameter.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        void InitializeQueryParametersFromPreset(Request request);

        /// <summary>
        /// Checks if the provides HTTP request query parameters are sane and may correct errors.
        /// </summary>
        /// <param name="request">Nancy HTTP request.</param>
        void SanitizeQueryParameters(Request request);

        /// <summary>
        /// Return a HTTP authentication client with a given state.
        /// </summary>
        /// <param name="state">A HTTP authentication client state.</param>
        /// <returns>The first client with the given state, <c>null</c> otherwise.</returns>
        IHttpAuthenticationClient TryGetAuthenticationClient(HttpAuthenticationClientState state);

        /// <summary>
        /// Return a HTTP authentication client with a given state.
        /// </summary>
        /// <typeparam name="T">A subclass of HttpAuthenticationClient.</typeparam>
        /// <param name="state">A HTTP authentication client state.</param>
        /// <returns>The first client with the given state, <c>null</c> otherwise.</returns>
        T TryGetAuthenticationClient<T>(HttpAuthenticationClientState state) where T : class;

        /// <summary>
        /// Return a HTTP authentication client from the 'authType'-query parameter the given HTTP request.
        /// </summary>
        /// <param name="state">A Nancy HTTP request.</param>
        /// <returns>The first client with the suitable URI, <c>null</c> otherwise.</returns>
        IHttpAuthenticationClient TryGetAuthenticationClient(Request request);

        /// <summary>
        /// Return a HTTP authentication client from the 'authType'-query parameter the given HTTP request.
        /// </summary>
        /// <typeparam name="T">A subclass of HttpAuthenticationClient.</typeparam>
        /// <param name="state">A Nancy HTTP request.</param>
        /// <returns>The first client with the suitable URI, <c>null</c> otherwise.</returns>
        T TryGetAuthenticationClient<T>(Request request) where T : class;

        /// <summary>
        /// Install an authenticated online account into the given model.
        /// </summary>
        /// <param name="model">The model in which the account should be created.</param>
        /// <returns>A newly created instance of the <c>OnlineAccount</c> class.</returns>
        OnlineAccount InstallAccount(IModel model);

        #endregion
    }
}
