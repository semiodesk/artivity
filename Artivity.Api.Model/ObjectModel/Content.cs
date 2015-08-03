using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.Content)]
    public class Content : Object
    {
        #region Constructor
        public Content(Uri uri)
            : base(uri)
        {
        }
        #endregion


    }
}
