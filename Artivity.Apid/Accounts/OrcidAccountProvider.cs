using Artivity.DataModel;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Accounts
{
    public class OrcidAccountProvider : OAuth2AccountProvider
    {
        #region Constructors

        public OrcidAccountProvider(string id) : base(id)
        {
            Url = "http://orcid.org";
            Title = "ORCiD";
            Description = "Connecting Research and Researchers.";
            ClientId = "APP-AF3XVP6X01AMZQH1";
            ClientSecret = "0c883177-cfd8-4a0f-a111-03a5ba19539d";
        }

        #endregion

        #region Methods

        public override string GetAuthorizationRequestUrl(string redirectUrl)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                LogError("OAuth2 redirect URL is empty.");
                return string.Empty;
            }

            RedirectUrl = redirectUrl;

            Status = "Waiting for access authorization from account server.";

            string url = "https://sandbox.orcid.org/oauth/authorize?client_id={0}&response_type=code&scope=/authenticate&redirect_uri={1}";

            return string.Format(url, ClientId, RedirectUrl);
        }

        public override void Authorize(IModel model, string code)
        {
            base.Authorize(model, code);

            Uri url = new Uri("https://pub.sandbox.orcid.org/oauth/token");

            NameValueCollection values = new NameValueCollection()
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUrl }
            };

            Status = "Authorizing access..";

            UploadValues(url, values);
        }

        protected override OnlineAccount OnCreateAccount()
        {
            if(ResponseData == null)
            {
                LogError("Cannot create account because response data is null.");
                return null;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = Encoding.UTF8.GetString(ResponseData);

            JObject response = JObject.Parse(json);

            OnlineAccount account = Model.CreateResource<OnlineAccount>();
            account.Id = response["orcid"].ToString();
            account.Title = Title;
            account.Description = string.Format("{0}/{1}", Url, account.Id);
            account.CreationTime = DateTime.UtcNow;
            account.LastModificationTime = account.CreationTime;

            return account;
        }

        #endregion
    }
}
