using Semiodesk.Trinity;
using System;

namespace Artivity.DataModel
{
	[RdfClass(PROV.EntityInfluence)]
	public class EntityInfluence : Influence
	{
		#region Constructors

		public EntityInfluence(Uri uri) : base(uri) {}

		#endregion
	}
}

