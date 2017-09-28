using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters()
        {
            foreach (KeyValuePair<string, string> parameter in base.GetPersistableAuthenticationParameters())
            {
                yield return parameter;
            }


            yield return new KeyValuePair<string, string>("token", Token);
            yield return new KeyValuePair<string, string>("tokenExpiration", this.TokenExpiration);
            yield return new KeyValuePair<string, string>("refreshToken", this.RefreshToken);
            yield return new KeyValuePair<string, string>("refreshTokenExpiration", this.RefreshTokenExpiration);
        }

        #endregion
    }
}
