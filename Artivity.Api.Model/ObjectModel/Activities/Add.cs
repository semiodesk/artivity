using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(AS2.Add)]
	public class Add : Activity
	{
		#region Constructors

		public Add(Uri uri) : base(uri) {}

		#endregion
	}
}
