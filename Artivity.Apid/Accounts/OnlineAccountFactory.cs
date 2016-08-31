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

using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public static class OnlineAccountFactory
    {
        #region Members

        public static bool IsInitialized = false;

        private static readonly Dictionary<string, IOnlineAccountProvider> _providers = new Dictionary<string, IOnlineAccountProvider>();

        #endregion

        #region Methods

        public static void Initialize()
        {
            if (IsInitialized) return;

            OnlineAccountFactory.RegisterProvider(new OrcidAccountProvider("orcid"));

            IsInitialized = true;
        }

        public static void RegisterProvider(IOnlineAccountProvider provider)
        {
            string id = provider.Id;

            if(_providers.ContainsKey(provider.Id))
            {
                string message = string.Format("Identifier {0} already registered by provider {1}", id, _providers[id]);
                throw new DuplicateKeyException(message);
            }

            Logger.LogInfo("Registered online account provider {0}", id);

            _providers[id] = provider;
        }

        public static IEnumerable<IOnlineAccountProvider> GetRegisteredProviders()
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            return _providers.Values;
        }

        public static IOnlineAccountProvider GetProvider(string id)
        {
            if(!IsInitialized)
            {
                Initialize();
            }

            return _providers[id];
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
