using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Close)]
	public class Close : Activity
	{
		#region Constructors

		public Close(Uri uri) : base(uri) {}

		#endregion
	}
}
