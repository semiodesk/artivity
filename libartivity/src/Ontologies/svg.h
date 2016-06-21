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

#define svg(label) "svg:"label;
#define SVG(label) "http://www.mathiswebs.com/ontology/svg_ont.owl#"label;

namespace artivity
{
    namespace svg
    {
		static const char* NS_PREFIX = svg("");
		static const char* NS_URI = SVG("");

        static const char* Shape = svg("Shape");
        
        static const char* Curved = svg("Curved");
        static const char* Circle = svg("Circle");
        static const char* Ellipse = svg("Ellipse");
        
        static const char* Polygon = svg("Polygon");
        static const char* Triangle = svg("Triangle");
        static const char* Quadrate = svg("Quadrate");
        static const char* Trapezoid = svg("Trapezoid");
        static const char* Parallelogram = svg("Parallelogram");
        static const char* Diamond = svg("Diamond");
        static const char* Rectangle = svg("Rectangle");
        static const char* Square = svg("Square");
        static const char* Pentagon = svg("Pentagon");
        static const char* Hexagon = svg("Hexagon");
        
        static const char* Predefined = svg("Predefined");
        static const char* Star = svg("Star");
        static const char* Heart = svg("Heart");
        static const char* Crescent = svg("Crescent");
        static const char* Sun = svg("Sun");
        
        static const char* id = svg("id");
    }
}

#endif // SVG_H
