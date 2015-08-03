using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.Experience)]
    public class Experience : Activity
    {
        #region Constructor
        public Experience(Uri uri)
            : base(uri)
        {}
        #endregion


    }
}
