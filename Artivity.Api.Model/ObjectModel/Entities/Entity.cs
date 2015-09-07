using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Entity)]
	public class Entity : Resource
	{
		#region Members

		[RdfProperty(PROV.wasGeneratedBy)]
		public Activity GenerationActivity { get; set; }

		[RdfProperty(PROV.generatedAtTime)]
		public DateTime GenerationTime { get; set; }

		[RdfProperty(PROV.invalidatedAtTime)]
		public DateTime InvalidationTime { get; set; }

		[RdfProperty(PROV.specializationOf)]
		public Entity GenericEntity { get; set; }

		[RdfProperty(PROV.wasRevisionOf)]
		public Entity RevisedEntitiy { get; set; }

		[RdfProperty(PROV.atLocation)]
		public Entity RevisedLocation { get; set; }

		#endregion

		#region Constructors

		public Entity(Uri uri) : base(uri) {}

		#endregion
	}
}

