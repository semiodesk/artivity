using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.Actor)]
    public class Actor : Object
    {
        #region Constructors

        public Actor(Uri uri) : base(uri) {}

        #endregion
    }
}
