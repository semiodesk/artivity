#ifndef CANVAS_H
#define CANVAS_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Rectangle.h"
#include "CoordinateSystem.h"

namespace artivity
{
    class Canvas;
    typedef boost::shared_ptr<Canvas> CanvasRef;

    class Canvas : public Rectangle
    {
    private:
        CoordinateSystemRef _coordinateSystem;
        
        ResourceRef _lengthUnit;
        
    public:
        Canvas() : Rectangle()
        {
            setType(art::Canvas);
        }
        
        Canvas(const char* uriref) : Rectangle(uriref)
        {
            setType(art::Canvas);
        }
        
        CoordinateSystemRef getCoordinateSystem()
        {
            return _coordinateSystem;
        }
        
        void setCoordinateSystem(CoordinateSystemRef coordinateSystem)
        {
            _coordinateSystem = coordinateSystem;
            
            setValue(art::coordinateSystem, coordinateSystem);
        }
        
        ResourceRef getLengthUnit()
        {
            return _lengthUnit;
        }
        
        void setLengthUnit(ResourceRef lengthUnit)
        {
            _lengthUnit = lengthUnit;
            
            setValue(art::lengthUnit, _lengthUnit);
        }
    };
}

#endif // CANVAS_H
