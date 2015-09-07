using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.View)]
	public class View : Activity
    {
        #region Constructors

		public View(Uri uri) : base(uri) {}

        #endregion
    }
}
