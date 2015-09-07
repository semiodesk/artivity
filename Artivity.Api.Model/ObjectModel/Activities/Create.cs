using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Create)]
	public class Create : Activity
	{
		#region Constructors

		public Create(Uri uri) : base(uri) {}

		#endregion
	}
}
