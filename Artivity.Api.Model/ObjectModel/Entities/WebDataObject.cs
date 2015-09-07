using Semiodesk.Trinity;
using System;
using Artivity.Model.ObjectModel;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(NFO.WebDataObject)]
	public class WebDataObject : Entity
	{
		#region Members

		[RdfProperty(NIE.title)]
		public string Title { get; set; }

		#endregion

		#region Constructors

		public WebDataObject(Uri uri) : base(uri) {}

		#endregion
	}
}

