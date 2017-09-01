using System;

namespace Artivity.Api.Parameters
{
    public class MarkParameter : Parameter
    {
        #region Members

        // URI of the mark.
        public string uri { get; set; }

        // Agent that created the mark.
        public string agent { get; set; }

        // Entity the mark is associated with.
        public string entity { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        // Text associated with the mark.
        public string label { get; set; }

        // Type of the geometry (i.e. rectangle, point)
        public string geometryType { get; set; }

        // Indicates how the mark geometry should be rendered.
        public dynamic geometry { get; set; }
        #endregion

        #region Methods

        public override bool Validate() { return true; }

        #endregion
    }
}
