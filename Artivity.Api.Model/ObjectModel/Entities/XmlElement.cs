using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artivity.Model.ObjectModel
{
    [RdfClass(XML.Element)]
	public class XmlElement : Entity
    {
		#region Members

		//[RdfProperty(XML.)]
		public string Id { get; set; }

		[RdfProperty(XML.localName)]
		public string LocalName { get; set; }

		#endregion

        #region Constructors

		public XmlElement(Uri uri) : base(uri) {}

        #endregion
    }
}
