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

using System;
using Eto.Drawing;
using OxyPlot;

namespace Artivity.Explorer
{
    public static class ColorExtensions
    {
        public static Color WithA(this Color color, float a)
        {
            return new Color(color) { A = a };
        }

        public static OxyColor ToOxyColor(this Color color)
        {
            byte a = Convert.ToByte(color.Ab);
            byte r = Convert.ToByte(color.Rb);
            byte g = Convert.ToByte(color.Gb);
            byte b = Convert.ToByte(color.Bb);

            return OxyColor.FromArgb(a, r, g, b);
        }

        public static Color ToEtoColor(this System.Drawing.Color color)
        {
            byte a = color.A;
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;

            return Color.FromArgb(r, g, b, a);
        }
    }
}

