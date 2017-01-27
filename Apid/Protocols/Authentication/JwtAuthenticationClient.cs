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
        public string Token
        {
            get
            {
                dynamic data = JObject.Parse(Encoding.UTF8.GetString(ResponseData));

                return data != null ? data.token : null;
            }
        }

        #endregion

        #region Constructors

        public JwtAuthenticationClient() : base("http://localhost:8272/artivity/api/1.0/auth/jwt")
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<KeyValuePair<string, string>> GetPersistableAuthenticationParameters()
        {
            foreach (KeyValuePair<string, string> parameter in base.GetPersistableAuthenticationParameters())
            {
                yield return parameter;
            }

            yield return new KeyValuePair<string, string>("token", Token);
        }

        #endregion
    }
}
