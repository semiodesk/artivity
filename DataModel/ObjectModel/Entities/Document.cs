using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(NFO.Document)]
    public class Document : Entity
    {
		#region Members

		#endregion

        #region Constructor

        public Document(Uri uri) : base(uri) {}

        #endregion
    }
}
