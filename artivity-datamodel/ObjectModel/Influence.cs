using Semiodesk.Trinity;
using System;

namespace Artivity.DataModel
{
	[RdfClass(PROV.Influence)]
	public class Influence : Resource
	{
		#region Constructors

		public Influence(Uri uri) : base(uri) {}

		#endregion
	}
}

