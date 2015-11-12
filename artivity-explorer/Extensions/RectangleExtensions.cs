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
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015
//
using System;
using Eto.Drawing;

namespace Artivity.Explorer
{
    public static class RectangleExtensions
    {
        public static RectangleF ToRectangle(this Artivity.DataModel.Rectangle r)
        {
            RectangleF rectangle = new RectangleF();
            rectangle.Location = r.Position.ToPoint();
            rectangle.Width = Convert.ToSingle(r.Width);
            rectangle.Height = Convert.ToSingle(r.Height);

            return rectangle;
        }

        public static double GetScalingFactor(this Artivity.DataModel.Rectangle r, RectangleF target)
        {
            double w = target.Width / r.Width;
            double h = target.Height / r.Height;

            return w >= h ? h : w;
        }

        public static RectangleF Fit(this Artivity.DataModel.Rectangle r, RectangleF target)
        {
            double s = r.GetScalingFactor(target);

            RectangleF result = new RectangleF();
            result.Width = Convert.ToSingle(Math.Round(r.Width * s, 0));
            result.Height = Convert.ToSingle(Math.Round(r.Height * s, 0));

            return result;
        }
    }
}

