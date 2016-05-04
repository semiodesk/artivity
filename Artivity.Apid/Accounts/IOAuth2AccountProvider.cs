using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    interface IOAuth2AccountProvider
    {
        #region Members

        string ClientId { get; set; }

        string ClientSecret { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get the URL where a user can log into a service and initiate the authorization process.
        /// </summary>
        /// <param name="redirectUrl"></param>
        string GetAuthorizationRequestUrl(string redirectUrl);

        /// <summary>
        /// Send the client credentials to the authorization server.
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="code"></param>
        void Authorize(IModel model, string code);

        #endregion
    }
}
