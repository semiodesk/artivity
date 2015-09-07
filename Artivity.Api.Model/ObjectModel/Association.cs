using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.Association)]
	public class Association : Resource
    {
		#region Members

		[RdfProperty(PROV.agent)]
		public Agent Agent { get; set; }

		[RdfProperty(PROV.hadRole)]
		public Role Role { get; set; }

		#endregion

        #region Constructor

		public Association(Uri uri) : base(uri) {}

        #endregion
    }
}
