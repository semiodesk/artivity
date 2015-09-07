using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.Activity)]
    public class Activity : Resource
    {
		#region Members

		[RdfProperty(PROV.qualifiedAssociation)]
		public List<Association> Associations { get; private set; }

		[RdfProperty(PROV.used)]
		public List<Resource> UsedEntities { get; private set; }

		[RdfProperty(PROV.invalidated)]
		public List<Resource> InvalidatedEntities { get; set; }

		[RdfProperty(PROV.generated)]
		public List<Resource> GeneratedEntitiies { get; set; }

		[RdfProperty(PROV.startedAtTime)]
		public DateTime StartTime { get; set; }

		[RdfProperty(PROV.endedAtTime)]
		public DateTime EndTime { get; set; }

		#endregion

        #region Constructors

        public Activity(Uri uri) : base(uri) {}

        #endregion
    }
}
