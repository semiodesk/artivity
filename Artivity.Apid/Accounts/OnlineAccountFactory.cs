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
