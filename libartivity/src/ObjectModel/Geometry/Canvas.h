#ifndef CANVAS_H
#define CANVAS_H

#include "../../Ontologies/rdf.h"
#include "../../Ontologies/art.h"
#include "Rectangle.h"
#include "CoordinateSystem.h"

namespace artivity
{
    class Canvas : public Rectangle
    {
    private:
        CoordinateSystem* _coordinateSystem;
        
        const Resource* _lengthUnit;
        
    public:
        Canvas() : Rectangle()
        {
            setValue(rdf::_type, art::Canvas);
        }
        
        Canvas(const char* uriref) : Rectangle(uriref)
        {
            setValue(rdf::_type, art::Canvas);
        }
        
        CoordinateSystem* getCoordinateSystem()
        {
            return _coordinateSystem;
        }
        
        void setCoordinateSystem(CoordinateSystem* coordinateSystem)
        {
            _coordinateSystem = coordinateSystem;
            
            setValue(art::coordinateSystem, coordinateSystem);
        }
        
        const Resource* getLengthUnit()
        {
            return _lengthUnit;
        }
        
        void setLengthUnit(const Resource* lengthUnit)
        {
            _lengthUnit = lengthUnit;
            
            setValue(art::lengthUnit, _lengthUnit);
        }
    };
}

#endif // CANVAS_H
