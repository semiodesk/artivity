using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.Document)]
    public class Document : Object
    {
        #region Constructor
        public Document(Uri uri)
            : base(uri)
        {
        }
        #endregion


    }
}
