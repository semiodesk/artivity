using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel
{
    [RdfClass(FOAF.OnlineAccount)]
    public class OnlineAccount : Resource
    {
        #region Members

        /// <summary>
        /// Indicates the identifier associated with this online account.
        /// </summary>
        /// <example>
        /// <foaf:accountName>jwales</foaf:accountName>
        /// </example>
        [RdfProperty(FOAF.accountName)]
        public string Id { get; set; }

        /// <summary>
        /// Indicates a homepage of the service provide for this online account.
        /// </summary>
        /// <example>
        /// <foaf:accountServiceHomepage rdf:resource="http://www.freenode.net/"/>
        /// </example>
        [RdfProperty(FOAF.accountServiceHomepage)]
        public string Url { get; set; }

        [RdfProperty(FOAF.title)]
        public string Title { get; set; }

        [RdfProperty(FOAF.description)]
        public string Description { get; set; }

        [RdfProperty(NAO.created)]
        public DateTime CreationTime { get; set; }

        [RdfProperty(NAO.lastModified)]
        public DateTime LastModificationTime { get; set; }

        #endregion

        #region Constructors

        public OnlineAccount(Uri uri) : base(uri) { }

        #endregion
    }
}
