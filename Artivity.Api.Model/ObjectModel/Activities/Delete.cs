using Semiodesk.Trinity;
using System;

namespace Artivity.Model
{
	[RdfClass(AS2.Delete)]
	public class Delete : Activity
	{
		#region Constructors

		public Delete(Uri uri) : base(uri) {}

		#endregion
	}
}
