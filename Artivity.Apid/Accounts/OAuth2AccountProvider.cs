using Nancy;
using Newtonsoft.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public abstract class OAuth2AccountProvider : OnlineAccountProvider, IOAuth2AccountProvider
    {
        #region Members

        [JsonIgnore]
        public string ClientId { get; set; }

        [JsonIgnore]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string GrantType { get; set; }

        protected string RedirectUrl { get; set; }

        protected byte[] ResponseData;

        #endregion

        #region Constructors

        public OAuth2AccountProvider(string id) : base(id)
        {
            // TODO: Remove magic string. Let the API module create an instance of OnlineAccountFactory with the proper links.
            InstallUrl = string.Format("http://localhost:8262/artivity/api/1.0/accounts/oauth2/redirect?providerId={0}", id);
        }

        #endregion

        #region Methods

        public abstract string GetAuthorizationRequestUrl(string redirectUrl);

        public virtual void Authorize(IModel model, string code)
        {
            if(model == null)
            {
                throw new ArgumentNullException("model");
            }

            Model = model;

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("OAuth2 authorization code must not be null or empty.");
            }
        }

        protected void UploadValues(Uri url, NameValueCollection data)
        {
            using (WebClient client = new WebClient())
            {
                client.UploadValuesCompleted += (object sender, UploadValuesCompletedEventArgs e) =>
                {
                    if (e.Error != null)
                    {
                        ResponseData = null;

                        Logger.LogError("OAuth2 authorization failed: {0}", e.Error.Message);
                    }
                    else
                    {
                        ResponseData = e.Result;

                        Logger.LogInfo("OAuth2 authorization success.");

                        TryCreateAccount();
                    }
                };

                client.UploadValuesAsync(url, "POST", data);
            }
        }

        #endregion
    }
}
