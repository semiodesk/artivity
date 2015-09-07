using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(XML.Attribute)]
    public class XmlAttribute : Entity
    {
		#region Members

		[RdfProperty(XML.ownerElement)]
		public XmlElement OwnerElement { get; set; }

		[RdfProperty(XML.localName)]
		public string LocalName { get; set; }
		
		#endregion

        #region Constructors

		public XmlAttribute(Uri uri) : base(uri) {}

        #endregion
    }
}
