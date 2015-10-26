using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.EntityInfluence)]
	public class EntityInfluence : Influence
	{
		#region Constructors

		public EntityInfluence(Uri uri) : base(uri) {}

		#endregion
	}
}

