using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Delete)]
	public class Delete : Activity
	{
		#region Constructors

		public Delete(Uri uri) : base(uri) {}

		#endregion
	}
}
