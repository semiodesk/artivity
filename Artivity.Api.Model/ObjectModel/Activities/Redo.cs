using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Redo)]
	public class Redo : Activity
	{
		#region Constructors

		public Redo(Uri uri) : base(uri) {}

		#endregion
	}
}
