using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.SoftwareAgent)]
    public class SoftwareAgent : Agent
    {
		#region Members

		[RdfProperty(ART.isCaptureEnabled)]
		public bool IsCaptureEnabled { get; set; }

		#endregion

        #region Constructors

		public SoftwareAgent(Uri uri) : base(uri) {}

        #endregion
    }
}
