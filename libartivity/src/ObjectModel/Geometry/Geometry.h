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

#ifndef _ART_GEOMETRY_H
#define _ART_GEOMETRY_H

#include "../../Ontologies/art.h"

#include "../Entity.h"

#include "Point.h"

namespace artivity
{
    class Geometry;

    typedef boost::shared_ptr<Geometry> GeometryRef;

    class Geometry : public Entity
    {        
    private:
        double _x;

        double _y;

        double _z;

    public:
        Geometry() : Entity()
        {
            setType(art::Geometry);
            _x = 0;
            _y = 0;
            _z = 0;
        }
        
        Geometry(const char* uriref) : Entity(uriref)
        {
            setType(art::Geometry);
            _x = 0;
            _y = 0;
            _z = 0;
        }

        void setPosition(double x, double y)
        {
            setX(x);
            setY(y);
        }

        double getX()
        {
            return _x;
        }

        void setX(double x)
        {
            _x = x;

            setValue(art::x, x);
        }

        double getY()
        {
            return _y;
        }

        void setY(double y)
        {
            _y = y;

            setValue(art::y, y);
        }

        double getZ()
        {
            return _z;
        }

        void setZ(double z)
        {
            _z = z;

            setValue(art::z, z);
        }
    };
}

#endif // _ART_GEOMETRY_H
