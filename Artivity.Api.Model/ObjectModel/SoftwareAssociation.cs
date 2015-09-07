using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.Association)]
    public class SoftwareAssociation : Association
    {
		#region Members

		[RdfProperty(ART.hadViewbox)]
		public Viewbox Viewbox { get; set; }

		#endregion

        #region Constructor

		public SoftwareAssociation(Uri uri) : base(uri) {}

        #endregion
    }
}
