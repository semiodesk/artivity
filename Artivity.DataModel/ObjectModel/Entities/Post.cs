using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel.ObjectModel.Entities
{
    [RdfClass(SIOC.Post)]
    public class Post : Entity
    {
        #region Members
        
        #endregion

        #region Constructor
        public Post(Uri uri) : base(uri) { }
        public Post(string uri) : base(uri) { }
        #endregion


    }
}
