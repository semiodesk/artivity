using Semiodesk.Trinity;
using System;
using System.Text;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Role)]
	public class Role : Agent
	{
		#region Constructors

		public Role(Uri uri) : base(uri) {}

		#endregion
	}
}
