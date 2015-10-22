#ifndef CUBE_H
#define CUBE_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Geometry.h"

namespace artivity
{
    class Cube : public Geometry
    {
    private:
        Point* _position;
        
        double _width;
        
        double _height;
        
        double _depth;
        
    public:
        Cube() : Geometry()
        {
            setValue(rdf::_type, art::Cube);
        }
        
        Cube(const char* uriref) : Geometry(uriref)
        {
            setValue(rdf::_type, art::Cube);
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
        
        double getDepth()
        {
            return _depth;
        }
        
        void setDepth(double depth)
        {
            _depth = depth;
            
            setValue(art::depth, depth);
        }
    };
}

#endif // CUBE_H
