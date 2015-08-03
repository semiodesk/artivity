using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.Object)]
    public class Object : Resource
    {
        #region Constructor
        public Object(Uri uri) : base(uri)
        {

        }
        #endregion

        #region Members

        [RdfProperty(AS2.alias)]
        public Resource Alias { get; set; }

        [RdfProperty(AS2.attachment)]
        public Resource Attachment { get; set; }

        [RdfProperty(AS2.attributedTo)]
        public Resource AttributedTo { get; set; }

        [RdfProperty(AS2.content)]
        public string Content { get; set; }

        [RdfProperty(AS2.context)]
        public Resource Context { get; set; }

        [RdfProperty(AS2.displayName)]
        public string DisplayName { get; set; }

        [RdfProperty(AS2.endTime)]
        public DateTime EndTime { get; set; }

        [RdfProperty(AS2.generator)]
        public Resource Generator { get; set; }

        [RdfProperty(AS2.icon)]
        public Resource Icon { get; set; }

        [RdfProperty(AS2.image)]
        public Resource Image { get; set; }

        [RdfProperty(AS2.inReplyTo)]
        public Resource InReplyTo { get; set; }

        [RdfProperty(AS2.location)]
        public Resource Location { get; set; }

        [RdfProperty(AS2.preview)]
        public Resource Preview { get; set; }

        [RdfProperty(AS2.published)]
        public DateTime Published { get; set; }

        //[RdfProperty(AS2.replies)]
        //public Collction replies { get; set; }

        [RdfProperty(AS2.scope)]
        public Resource Scope { get; set; }

        [RdfProperty(AS2.startTime)]
        public DateTime StartTime { get; set; }

        [RdfProperty(AS2.summary)]
        public string Summary { get; set; }

        [RdfProperty(AS2.tag)]
        public List<Resource> Tags { get; set; }

        [RdfProperty(AS2.title)]
        public string Title { get; set; }

        [RdfProperty(AS2.updated)]
        public DateTime Updated { get; set; }

        [RdfProperty(AS2.url)]
        public List<Resource> Url { get; set; }

        [RdfProperty(AS2.to)]
        public Resource To { get; set; }

        [RdfProperty(AS2.bto)]
        public Resource BTo { get; set; }
            
        [RdfProperty(AS2.cc)]
        public Resource CC { get; set; }

        [RdfProperty(AS2.bcc)]
        public Resource BCC { get; set; }

        #endregion
    }
}
