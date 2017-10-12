using Artivity.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Protocols.Authentication
{
    public class JwtAuthenticationClient : BasicAuthenticationClient
    {
        #region Members

        [JsonIgnore]
        public string Token  { get; set; }

        [JsonIgnore]
        public string TokenExpiration { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public string RefreshTokenExpiration { get; set; }

        public string RefreshPath { get; set; }

        #endregion

        #region Constructors

        public JwtAuthenticationClient() : base("http://localhost:8272/artivity/api/1.0/auth/jwt")
        {
        }

        #endregion

        #region Methods

        protected override void ParseData(byte[] responseData)
        {
            base.ParseData(responseData);

            string response = Encoding.UTF8.GetString(ResponseData);

            dynamic data = JObject.Parse(response);

            if( data != null )
            {
                Token = data.token;
                TokenExpiration = data.tokenExpiration.ToString();
                RefreshToken = data.refreshToken;
                RefreshTokenExpiration = data.refreshTokenExpiration.ToString();


            }
        }

        private const string TokenKey = "token";
        private const string TokenExpirationKey = "tokenExpiration";
        private const string RefreshTokenKey = "refreshToken";
        private const string RefreshTokenExpirationKey = "refreshTokenExpiration";

        public override IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters()
        {
            foreach (KeyValuePair<string, string> parameter in base.GetPersistableAuthenticationParameters())
            {
                yield return parameter;
            }

            yield return new KeyValuePair<string, string>(TokenKey, Token);
            yield return new KeyValuePair<string, string>(TokenExpirationKey, this.TokenExpiration);
            yield return new KeyValuePair<string, string>(RefreshTokenKey, this.RefreshToken);
            yield return new KeyValuePair<string, string>(RefreshTokenExpirationKey, this.RefreshTokenExpiration);
        }

        internal async Task<bool> ValidateAccount(OnlineAccount account)
        {
            var token = account.GetParameter("token");
            var tokenExpirationString = account.GetParameter(TokenExpirationKey);
            DateTime tokenExpiration = DateTime.MinValue;
            if (!string.IsNullOrEmpty(token) && DateTime.TryParse(tokenExpirationString, out tokenExpiration))
            {
                if (tokenExpiration > DateTime.Now)
                    return true;

                var refreshToken = account.GetParameter(RefreshTokenKey);
                var refreshTokenExpirationString = account.GetParameter(RefreshTokenExpirationKey);
                DateTime refreshTokenExpiration = DateTime.MinValue;
                if (!string.IsNullOrEmpty(refreshToken) && DateTime.TryParse(refreshTokenExpirationString, out refreshTokenExpiration))
                {
                    if( refreshTokenExpiration > DateTime.Now )
                        return await UpdateToken(account);
                }
            }

            return false;
        }

        async Task<bool> UpdateToken(OnlineAccount account)
        {
            HttpClient client = new HttpClient();
            string content = JsonConvert.SerializeObject(new Dictionary<string, string>(){{"token", account.GetParameter("refreshToken")}});
            var c = new StringContent(content, UTF8Encoding.UTF8, "application/json");
            var refreshUri = new Uri(account.ServiceUrl.Uri, RefreshPath);
            var res = await client.PostAsync(refreshUri, c);

            if (res.IsSuccessStatusCode)
            {
                var ResponseData = await res.Content.ReadAsByteArrayAsync();
                ParseData(ResponseData);
                account.SetParameter(TokenKey, Token);
                account.SetParameter(TokenExpirationKey, TokenExpiration);
                account.SetParameter(RefreshTokenKey, RefreshToken);
                account.SetParameter(RefreshTokenExpirationKey, RefreshTokenExpiration);
                return true;
            }
            return false;
        }

        #endregion

        
    }
}
