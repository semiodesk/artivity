using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Update)]
	public class Update : Activity
	{
		#region Constructors

		public Update(Uri uri) : base(uri) {}

		#endregion
	}
}
