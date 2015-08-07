using Semiodesk.Trinity;
using System;

namespace Artivity.Model
{
	[RdfClass(AS2.Undo)]
	public class Undo : Activity
	{
		#region Constructors

		public Undo(Uri uri) : base(uri) {}

		#endregion
	}
}
