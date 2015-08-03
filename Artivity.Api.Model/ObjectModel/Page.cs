using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.Page)]
    public class Page : Object
    {
        #region Constructor
        public Page(Uri uri)
            : base(uri)
        {
        }
        #endregion
    }
}
