using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Protocols.Authentication
{
    public class HttpAuthenticationParameterSet
    {
        #region Members

        public string Id { get; set; }

        public Dictionary<string, string> Parameters { get; private set; }

        #endregion

        #region Constructors

        public HttpAuthenticationParameterSet()
        {
            Parameters = new Dictionary<string, string>();
        }

        #endregion
    }
}
