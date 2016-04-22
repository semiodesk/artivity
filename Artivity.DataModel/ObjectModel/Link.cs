using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.Link)]
    public class Link : Resource
    {
        #region Constructor
        public Link(Uri uri)
            : base(uri)
        {}
        #endregion

        #region Members
        [RdfProperty(AS2.href)]
        public Resource Href { get; set; }

        [RdfProperty(AS2.rel)]
        public List<string> Rel { get; set; }

        [RdfProperty(AS2.mediaType)]
        public string MediaType { get; set; }

        [RdfProperty(AS2.displayName)]
        public string DisplayName { get; set; }

        [RdfProperty(AS2.hreflang)]
        public string HrefLang { get; set; }

        [RdfProperty(AS2.width)]
        public uint Width { get; set; }

        [RdfProperty(AS2.height)]
        public uint Height { get; set; }

        [RdfProperty(AS2.duration)]
        public int Duration { get; set; }
        #endregion

    }
}
