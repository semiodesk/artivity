using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Api.Platforms
{
    public class UserConfig
    {
        #region Members

        [JsonIgnore]
        public bool IsNew { get; set; }

        public string Uid { get; set; }

        #endregion

        #region Methods

        public string GetUserId()
        {
            if (Uid.StartsWith("urn:art:uid:"))
            {
                return Uid.Substring(Uid.LastIndexOf(':') + 1);
            }
            else
            {
                return Uid;
            }
        }

        #endregion
    }
}
