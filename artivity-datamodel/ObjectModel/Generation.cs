using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Generation)]
	public class Generation : ActivityInfluence
	{
		#region Constructors

		public Generation(Uri uri) : base(uri) {}

		#endregion
	}
}

