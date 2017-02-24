// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using Semiodesk.Trinity;

namespace Artivity.DataModel
{
    [RdfClass(ART.Rectangle)]
    public class Rectangle : Geometry
    {
        #region Members

        [RdfProperty(ART.x)]
        public double x { get; set; }

        [RdfProperty(ART.y)]
        public double y { get; set; }

        [RdfProperty(ART.width)]
        public double Width { get; set; }

        [RdfProperty(ART.height)]
        public double Height { get; set; }

        #endregion

        #region Constructors

        public Rectangle(Uri uri) : base(uri) {}

        #endregion
    }

    [RdfClass(ART.Rectangle)]
    public class RectangleEntity : Entity
    {
        #region Members

        [RdfProperty(ART.x)]
        public double x { get; set; }

        [RdfProperty(ART.y)]
        public double y { get; set; }

        [RdfProperty(ART.width)]
        public double Width { get; set; }

        [RdfProperty(ART.height)]
        public double Height { get; set; }

        #endregion

        #region Constructors

        public RectangleEntity(Uri uri) : base(uri) { }

        #endregion
    }
}

