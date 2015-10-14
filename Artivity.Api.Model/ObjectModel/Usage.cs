using Semiodesk.Trinity;
using System;

namespace Artivity.Model.ObjectModel
{
	[RdfClass(PROV.Usage)]
	public class Usage : EntityInfluence
	{
        #region Members

        [RdfProperty(PROV.entity)]
        public Entity Entity { get; set; }

        [RdfProperty(PROV.atTime)]
        public DateTime Time { get; set; }

        #endregion

		#region Constructors

		public Usage(Uri uri) : base(uri) {}

		#endregion
	}
}

