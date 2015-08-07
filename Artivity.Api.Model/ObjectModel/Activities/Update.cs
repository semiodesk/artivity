using Semiodesk.Trinity;
using System;

namespace Artivity.Model
{
	[RdfClass(AS2.Update)]
	public class Update : Activity
	{
		#region Constructors

		public Update(Uri uri) : base(uri) {}

		#endregion
	}
}
