using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(ART.RenderingDataObject)]
    public class RenderingDataObject : FileDataObject
    {
        #region Constructors

        public RenderingDataObject(Uri uri) : base(uri) {  }

        public RenderingDataObject(string uri) : base(uri) { }

        #endregion
    }
}
