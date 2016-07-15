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

        public List<string> SoftwarePaths { get; set; }

        #endregion

        #region Methods

        public UserConfig()
        {
            SoftwarePaths = new List<string>();
        }

        public string GetUserId()
        {
            if (Uid.StartsWith("urn:art:uid:", StringComparison.InvariantCulture))
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
