using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.IntransitiveActivity)]
    public class IntransitiveActivity : Activity
    {
		#region Members
		
		public new Resource Origin
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		#endregion

        #region Constructor

        public IntransitiveActivity(Uri uri) : base(uri) {}

        #endregion
    }
}
