using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(AS2.Remove)]
	public class Remove : Activity
	{
		#region Constructors

		public Remove(Uri uri) : base(uri) {}

		#endregion
	}
}
