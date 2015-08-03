using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.Person)]
    public class Person : Object
    {
        #region Constructor
        public Person(Uri uri)
            : base(uri)
        {}
        #endregion

        #region Members

        #endregion

    }
}
