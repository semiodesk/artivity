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

#ifndef UO_H
#define UO_H

#include "../Property.h"

// Source: http://qudt.org/1.1/vocab/OVG_units-qudt-(v1.1).ttl
#define UNIT(label) "http://qudt.org/vocab/unit#" label;

namespace artivity
{
    namespace unit
    {
		static const char* NS_PREFIX = "unit:";
		static const char* NS_URI = "http://qudt.org/vocab/unit#";

        static const char* Millimeter = UNIT("Millimeter");
        static const char* Centimeter = UNIT("Centimeter");
        static const char* Meter = UNIT("Meter");
        static const char* Point = UNIT("Point");
        static const char* Inch = UNIT("Inch");
        static const char* Foot = UNIT("Foot");
    }
}

#endif // UO_H
