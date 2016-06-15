using Semiodesk.Trinity;
using System;

namespace Artivity.DataModel
{
	[RdfClass(PROV.ActivityInfluence)]
	public class ActivityInfluence : Influence
	{
		#region Members

		[RdfProperty(PROV.activity)]
		public Activity Activity { get; set; }

		[RdfProperty(PROV.atTime)]
		public DateTime Time { get; set; }

		[RdfProperty(PROV.atLocation)]
		public Entity Location { get; set; }

		[RdfProperty(PROV.value)]
		public string Value { get; set; }

		[RdfProperty(ART.viewport)]
		public Viewport Viewport { get; set; }

        [RdfProperty(ART.boundaries)]
        public Geometry Boundaries { get; set; }

        [RdfProperty(DCES.description)]
        public Geometry Description { get; set; }

		#endregion

		#region Constructors

		public ActivityInfluence(Uri uri) : base(uri) {}

		#endregion
	}
}

