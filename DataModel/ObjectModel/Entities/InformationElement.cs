using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(NIE.InformationElement)]
    public class InformationElement : Entity
    {
        #region Members

        [RdfProperty(NIE.isStoredAs)]
        public FileDataObject DataObject { get; set; }

        #endregion

        #region Constructors

        public InformationElement(Uri uri) : base(uri) { }

        #endregion
    }
}
