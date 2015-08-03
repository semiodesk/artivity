using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(AS2.View)]
    public class View : Experience
    {
        #region Constructor
        public View(Uri uri)
            : base(uri)
        {}
        #endregion
    }
}
