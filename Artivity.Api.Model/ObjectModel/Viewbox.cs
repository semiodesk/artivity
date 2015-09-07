using System;
using Semiodesk.Trinity;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(ART.Viewbox)]
	public class Viewbox : Resource
	{
		#region Members

		[RdfProperty(ART.left)]
		public double Left { get; set; }

		[RdfProperty(ART.right)]
		public double Right { get; set; }

		[RdfProperty(ART.top)]
		public double Top { get; set; }

		[RdfProperty(ART.bottom)]
		public double Bottom { get; set; }

		[RdfProperty(ART.zoomFactor)]
		public double ZoomFactor { get; set; }

		#endregion

		#region Constructors

		public Viewbox(Uri uri) : base(uri) {}

		#endregion
	}
}
