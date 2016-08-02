using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(ART.Comment)]
    public class Comment : Communication
    {
        #region Members

        [RdfProperty(NAO.creator)]
        public Agent Author { get; set; }

        [RdfProperty(PROV.atTime)]
        public DateTime Time { get; set; }

        [RdfProperty(RDFS.comment)]
        public string Message { get; set; }

        #endregion

        #region Constructors

        public Comment(Uri uri)
            : base(uri)
        {
        }

        #endregion
    }
}
