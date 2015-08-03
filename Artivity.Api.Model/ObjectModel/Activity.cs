using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Model
{
    [RdfClass(AS2.Activity)]
    public class Activity : Object
    {
        #region Constructor
        public Activity(Uri uri)
            : base(uri)
        {}
        #endregion

        #region Members
        [RdfProperty(AS2.actor)]
        public Resource Actor { get; set; }

        [RdfProperty(AS2._object)]
        public Resource Object { get; set; }

        [RdfProperty(AS2.target)]
        public Resource Target { get; set; }

        [RdfProperty(AS2.result)]
        public Resource Result { get; set; }

        [RdfProperty(AS2.origin)]
        public Resource Origin { get; set; }

        [RdfProperty(AS2.instrument)]
        public Resource Instrument { get; set; }

        [RdfProperty(AS2.priority)]
        public float Priority { get; set; }

        #endregion

    }
}
