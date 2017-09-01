using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semiodesk.Trinity;

namespace Artivity.DataModel
{
    [RdfClass(NAO.Tag)]
    public class Tag : Resource
    {
        #region Members

        /// <summary>
        /// The unique label of the tag.
        /// </summary>
        [RdfProperty(NAO.prefLabel)]
        public string Label { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// A marker class for Things that are used to categorize documents (or other things).
        /// </summary>
        /// <param name="uri">A Uniform Resource Identifier.</param>
        public Tag(UriRef uri) : base(uri)
        {
        }

        #endregion
    }
}
