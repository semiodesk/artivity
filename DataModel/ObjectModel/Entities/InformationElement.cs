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
        public InformationElement(Uri uri) : base(uri) { }
        public InformationElement(string uri) : base(uri) { }

        [RdfProperty(NIE.isStoredAs)]
        public FileDataObject DataObject { get; set; }
    }
}
