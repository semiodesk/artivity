using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(PROV.Communication)]
    public class Communication : Resource
    {
        #region Members

        [RdfProperty(PROV.activity)]
        public Activity Activity { get; set; }

        #endregion

        #region Constructors

        public Communication(Uri uri) : base(uri)
        {
        }

        #endregion
    }
}
