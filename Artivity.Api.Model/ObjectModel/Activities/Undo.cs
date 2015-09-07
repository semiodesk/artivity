using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Undo)]
	public class Undo : Activity
	{
		#region Constructors

		public Undo(Uri uri) : base(uri) {}

		#endregion
	}
}
