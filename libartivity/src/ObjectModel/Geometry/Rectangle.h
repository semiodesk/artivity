#ifndef RECTANGLE_H
#define RECTANGLE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Geometry.h"
#include "Point.h"

namespace artivity
{
    class Rectangle : public Geometry
    {
    private:
        Point* _position;
        
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
        
        Point* getPosition()
        {
            return _position;
        }
        
        void setPosition(Point* position)
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
