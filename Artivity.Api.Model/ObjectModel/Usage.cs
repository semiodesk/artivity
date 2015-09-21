using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Usage)]
	public class Usage : EntityInfluence
	{
		#region Constructors

		public Usage(Uri uri) : base(uri) {}

		#endregion
	}
}

