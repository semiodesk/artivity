using Semiodesk.Trinity;
using System;

namespace Artivity.DataModel
{
	[RdfClass(PROV.Invalidation)]
	public class Invalidation : ActivityInfluence
	{
		#region Constructors

		public Invalidation(Uri uri) : base(uri) {}

		#endregion
	}
}

