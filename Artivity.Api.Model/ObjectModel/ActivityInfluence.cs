using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
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

		[RdfProperty(ART.hadViewbox)]
		public Viewbox Viewbox { get; set; }

		#endregion

		#region Constructors

		public ActivityInfluence(Uri uri) : base(uri) {}

		#endregion
	}
}

