using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Generation)]
	public class Generation : ActivityInfluence
	{
		#region Members

		[RdfProperty(PROV.activity)]
		public Activity Activity { get; set; }

		[RdfProperty(PROV.atTime)]
		public DateTime Time { get; set; }

		[RdfProperty(PROV.atLocation)]
		public Entity Location { get; set; }

		[RdfProperty(ART.hadViewbox)]
		public Viewbox Viewbox { get; set; }

		#endregion

		#region Constructors

		public Generation(Uri uri) : base(uri) {}

		#endregion
	}
}

