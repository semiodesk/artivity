using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{
    public class MarkParameter : Parameter
    {
        #region Members

        public string uri { get; set; }

        public string agent { get; set; }

        public string entity { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        // TODO: Encapsulate the following properties into a geometry parameter.
        public double x { get; set; }

        public double y { get; set; }

        public double width { get; set; }

        public double height { get; set; }

        #endregion

        #region Methods

        public override bool Validate() { return true; }

        #endregion
    }
}
