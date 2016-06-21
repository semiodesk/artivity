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

#ifndef _ART_RECTANGLE_H
#define _ART_RECTANGLE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Geometry.h"
#include "Point.h"

namespace artivity
{
    class Rectangle;

    typedef boost::shared_ptr<Rectangle> RectangleRef;

    class Rectangle : public Geometry
    {
    private:
        PointRef _position;
        
        double _width;
        
        double _height;
        
    public:
        Rectangle() : Geometry()
        {
            setType(art::Rectangle);
        }
        
        Rectangle(const char* uriref) : Geometry(uriref)
        {


            setType(art::Rectangle);
        }

        Rectangle(double top, double left, double right, double bottom)
        {
            _position = PointRef(new Point(top, left));
            _width = right - left;
            _height = top - bottom;
        }

        Rectangle(PointRef pos, double width, double height)
        {
            _position = pos;
            _width = width;
            _height = height;
        }
        
        PointRef getPosition()
        {
            return _position;
        }
        
		void setPosition(double x, double y)
		{
			_position = PointRef(new Point(x, y));

			setValue(art::position, _position);
		}

        void setPosition(PointRef position)
        {
            _position = position;
            
            setValue(art::position, position);
        }
        
        double getWidth()
        {
            return _width;
        }
        
        void setWidth(double width)
        {
            _width = width;
            
            setValue(art::width, width);
        }
        
        double getHeight()
        {
            return _height;
        }
        
        void setHeight(double height)
        {
            _height = height;
            
            setValue(art::height, height);
        }
    };
}

#endif // _ART_RECTANGLE_H
