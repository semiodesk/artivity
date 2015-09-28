using System;
using Semiodesk.Trinity;

namespace Artivity.Model
{
    [RdfClass(ART.Dimensions)]
    public class Dimensions : Resource
    {
        #region Members

        [RdfProperty(ART.x)]
        public double X { get; set; }

        [RdfProperty(ART.y)]
        public double Y { get; set; }

        [RdfProperty(ART.z)]
        public double Z { get; set; }

        [RdfProperty(ART.unit)]
        public Resource Unit { get; set; }

        #endregion

        #region Constructors

        public Dimensions(Uri uri) : base(uri) {}

        #endregion
    }
}

