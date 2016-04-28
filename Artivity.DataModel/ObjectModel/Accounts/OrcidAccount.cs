using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    public class OrcidAccount : OnlineAccount
    {
        #region Constructors

        public OrcidAccount(Uri uri) : base(uri) 
        {
            Title = "ORCID Account";
            ServiceUrl = "http://orcid.org";
        }

        #endregion
    }
}
