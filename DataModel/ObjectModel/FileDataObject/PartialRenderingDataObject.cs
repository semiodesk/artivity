using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(ART.PartialRenderingDataObject)]
    public class PartialRenderingDataObject : RenderingDataObject
    {
        #region Members

        [RdfProperty(ART.region)]
        public Rectangle Region { get; set; }

        #endregion

        #region Constructors

        public PartialRenderingDataObject(Uri uri) : base(uri) { }

        public PartialRenderingDataObject(string uri) : base(uri) { }

        #endregion
    }
}
