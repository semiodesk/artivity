using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(FOAF.OnlineAccount)]
    public class OnlineAccount : Resource
    {
        #region Members

        [RdfProperty(FOAF.accountName)]
        public string Name { get; set; }

        [RdfProperty(FOAF.accountServiceHomepage)]
        public string Website { get; set; }

        #endregion

        #region Constructors

        public OnlineAccount(Uri uri) : base(uri) { }

        #endregion
    }
}
