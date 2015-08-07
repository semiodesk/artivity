using Semiodesk.Trinity;
using System;

namespace Artivity.Model
{
	[RdfClass(AS2.Create)]
	public class Create : Activity
	{
		#region Constructors

		public Create(Uri uri) : base(uri) {}

		#endregion
	}
}
