#ifndef RECTANGLE_H
#define RECTANGLE_H

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

#endif // RECTANGLE_H
