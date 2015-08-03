using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.Application)]
    public class Application : Actor
    {
        #region Constructor
        public Application(Uri uri)
            : base(uri)
        {}
        #endregion
    }
}
