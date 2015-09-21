using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.ActivityInfluence)]
	public class ActivityInfluence : Influence
	{
		#region Constructors

		public ActivityInfluence(Uri uri) : base(uri) {}

		#endregion
	}
}

