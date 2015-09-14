using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(NFO.VectorImage)]
    public class VectorImage : Document
    {
        #region Constructors

		public VectorImage(Uri uri) : base(uri) {}

        #endregion
    }
}
