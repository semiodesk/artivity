using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(ART.OnlineServiceConnector)]
    public class OnlineServiceConnector : Resource
    {
        #region Constructors

        public OnlineServiceConnector(Uri uri) : base(uri)
        {
        }

        #endregion
    }
}
