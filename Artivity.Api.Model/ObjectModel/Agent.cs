using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(PROV.Agent)]
    public class Agent : Resource
    {
		#region Members

		[RdfProperty(RDFS.label)]
		string Label { get; set; }

		#endregion

        #region Constructors

		public Agent(Uri uri) : base(uri) {}

        #endregion
    }
}
