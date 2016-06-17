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
// Copyright (c) Semiodesk GmbH 2016

#ifndef SVG_H
#define SVG_H

#define SVG(label) "http://www.mathiswebs.com/ontology/svg_ont.owl#"label;

namespace artivity
{
    namespace svg
    {
        static const char* Shape = SVG("Shape");
        
        static const char* Curved = SVG("Curved");
        static const char* Circle = SVG("Circle");
        static const char* Ellipse = SVG("Ellipse");
        
        static const char* Polygon = SVG("Polygon");
        static const char* Triangle = SVG("Triangle");
        static const char* Quadrate = SVG("Quadrate");
        static const char* Trapezoid = SVG("Trapezoid");
        static const char* Parallelogram = SVG("Parallelogram");
        static const char* Diamond = SVG("Diamond");
        static const char* Rectangle = SVG("Rectangle");
        static const char* Square = SVG("Square");
        static const char* Pentagon = SVG("Pentagon");
        static const char* Hexagon = SVG("Hexagon");
        
        static const char* Predefined = SVG("Predefined");
        static const char* Star = SVG("Star");
        static const char* Heart = SVG("Heart");
        static const char* Crescent = SVG("Crescent");
        static const char* Sun = SVG("Sun");
        
        static const char* id = SVG("id");
    }
}

#endif // SVG_H
