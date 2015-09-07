using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Open)]
	public class Open : Activity
	{
		#region Constructors

		public Open(Uri uri) : base(uri) {}

		#endregion
	}
}
